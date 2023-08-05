using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Serilog;
using YouTubeCommentsFetcher.Worker.Configurations;
using YouTubeCommentsFetcher.Worker.Services;
using YouTubeCommentsFetcher.Worker.Services.Transformer;
using MassTransit;
using YouTubeCommentsFetcher.Worker.Consumers;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
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
                    
                    services.AddTransient<FetcherErrorEventConsumer>();
                    services.AddSingleton<RetryPolicyProvider>();
                    services.AddSingleton<IYouTubeFetcherService, YouTubeFetcherService>(); // Fetches comments
                    services.AddAutoMapper(typeof(MappingConfig));
                    services.AddTransient<ICommentTransformer, CommentThreadToFetchedEventTransformer>(); // Transforms comments to DTO
                    services.AddTransient<IYouTubeFetcherServiceExceptionHandler, YouTubeFetcherServiceExceptionHandler>();
                    services.AddTransient<CommentsFetchReceivedEventConsumer>();
                    services.AddTransient<IEventPublisher, EventPublisher>();  // Publish events
                    services.AddTransient<ICommentsIntegrationOrchestrator, CommentsIntegrationOrchestrator>();  // Orchestrates flows
                    
                    services.AddMassTransit(configurator =>
                    {
                        // Registering the VideoIdConsumer
                        configurator.AddConsumer<CommentsFetchReceivedEventConsumer>();
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
                                e.Consumer<CommentsFetchReceivedEventConsumer>(context);
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
                logger.LogInformation("-----------YouTubeCommentsFetcher service settings----------------");
                logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
                logger.LogInformation("API KEY: {ApiKey}", youTubeConfig.ApiKey);
                logger.LogInformation("RABBITMQ HOST: {ApiKey}", rabbitMqConfig.Hostname);
                logger.LogInformation("------------------------------------------------------------------");
            await host.RunAsync();
        }
    }
}