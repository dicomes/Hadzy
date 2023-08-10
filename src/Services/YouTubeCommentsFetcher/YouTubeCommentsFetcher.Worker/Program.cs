using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Serilog;
using YouTubeCommentsFetcher.Worker.Configurations;
using YouTubeCommentsFetcher.Worker.Services;
using YouTubeCommentsFetcher.Worker.Services.Mapper;
using MassTransit;
using YouTubeCommentsFetcher.Worker.Consumers;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var seqConfig = new SeqConfig();
            var youTubeConfig = new YouTubeConfig();
            var rabbitMqConfig = new RabbitMqConfig();
            
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environment = hostingContext.HostingEnvironment;

                    // Add user secrets in development environment
                    if (environment.IsDevelopment())
                    {
                        config.AddUserSecrets<Program>();
                    }
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    // Services registration
                    services.AddHostedService<Worker>();
                    
                    seqConfig = hostingContext.Configuration.GetSection("Seq").Get<SeqConfig>();
                    youTubeConfig = hostingContext.Configuration.GetSection("YouTube").Get<YouTubeConfig>();
                    rabbitMqConfig = hostingContext.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();

                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.Seq(seqConfig.Url)
                        .WriteTo.Console()
                        .CreateLogger();
                    
                    services.AddSingleton<YouTubeService>(sp => 
                    {
                        return new YouTubeService(new BaseClientService.Initializer
                        {
                            ApiKey = youTubeConfig.ApiKey
                        });
                    });
                    
                    services.AddSingleton<RetryPolicyProvider>(); // Retry policy with exponential backoff for fetcher
                    services.AddTransient<FetcherErrorEventConsumer>(); // Consumes errors raised by the fetcher
                    services.AddSingleton<IYouTubeFetcherService, YouTubeFetcherService>(); // Fetches comments for a given videoId
                    services.AddAutoMapper(typeof(MappingConfig)); // Mapping config for mapper
                    services.AddTransient<ICommentMapper, CommentThreadToFetchedEventMapper>(); // Transforms fetch comments to commentsDto
                    services.AddTransient<IYouTubeFetcherServiceExceptionHandler, YouTubeFetcherServiceExceptionHandler>();
                    services.AddTransient<FetchingInitiatedEventConsumer>(); // Consume events that initiate fetching for a given videoId
                    services.AddTransient<IEventPublisher, EventPublisher>();  // Publish events
                    services.AddTransient<ICommentsIterator, CommentsIterator>();
                    services.AddTransient<IFetchEventsIntegration, FetchEventsIntegration>();  // Integrate fetching events
                    
                    services.AddMassTransit(configurator =>
                    {
                        // Registering the VideoIdConsumer
                        configurator.AddConsumer<FetchingInitiatedEventConsumer>();
                        configurator.AddConsumer<FetcherErrorEventConsumer>();

                        configurator.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(new Uri(rabbitMqConfig.Hostname), configure =>
                            {
                                configure.Username(rabbitMqConfig.User);
                                configure.Password(rabbitMqConfig.Password);
                            });

                            // Configuring the ReceiveEndpoint to bind to a specific queue and use the VideoIdConsumer
                            cfg.ReceiveEndpoint("videoId-queue", e =>
                            {
                                e.Consumer<FetchingInitiatedEventConsumer>(context);
                            });
                                
                            // Configuring the ReceiveEndpoint for ErrorMessageConsumer
                            cfg.ReceiveEndpoint("error-message-queue", e =>
                            {
                                e.Consumer<FetcherErrorEventConsumer>(context);
                            });
                        });
                    });
                })
                .UseSerilog()
                .Build();
                var logger = host.Services.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("YouTubeCommentsFetcher service settings-------------------->");
                logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
                logger.LogInformation("API KEY: {ApiKey}", youTubeConfig.ApiKey);
                logger.LogInformation("RABBITMQ HOST: {ApiKey}", rabbitMqConfig.Hostname);
                logger.LogInformation("<--------------------YouTubeCommentsFetcher service settings");
            await host.RunAsync();
        }
    }
}