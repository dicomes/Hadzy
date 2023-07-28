using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Serilog;
using YouTubeCommentsFetcher.Worker.Configurations;
using YouTubeCommentsFetcher.Worker.Services;
using YouTubeCommentsFetcher.Worker.Services.Fetcher;
using YouTubeCommentsFetcher.Worker.Services.Transformer;
using MassTransit;
using YouTubeCommentsFetcher.Worker.Consumers;

namespace YouTubeCommentsFetcher.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
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
                    
                    var seqConfig = hostingContext.Configuration.GetSection("Seq").Get<SeqConfig>();
                    var youTubeConfig = hostingContext.Configuration.GetSection("YouTube").Get<YouTubeConfig>();
                    var rabbitMqConfig = hostingContext.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>(); 

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
                    
                    services.AddMassTransit(configurator =>
                    {
                        // Registering the VideoIdConsumer
                        configurator.AddConsumer<VideoIdConsumer>();

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
                                e.ConfigureConsumer<VideoIdConsumer>(context);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                    services.AddSingleton<RetryPolicyProvider>();
                    services.AddTransient<IFetcherService, YouTubeFetcherService>(); // Fetches comments
                    services.AddAutoMapper(typeof(MappingConfig));
                    services.AddTransient<ICommentTransformer, CommentThreadToDtoTransformer>(); // Transforms comments to DTO
                    services.AddSingleton<ICommentsService, CommentsService>();

                })
                .UseSerilog()
                .Build();

            await host.RunAsync();
        }
    }
}