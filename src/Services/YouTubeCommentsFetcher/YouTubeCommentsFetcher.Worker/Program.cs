using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Serilog;
using YouTubeCommentsFetcher.Worker.Configurations;
using YouTubeCommentsFetcher.Worker.Services;
using YouTubeCommentsFetcher.Worker.Services.Fetcher;
using YouTubeCommentsFetcher.Worker.Services.Transformer;

namespace YouTubeCommentsFetcher.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
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
                    services.AddHostedService<Worker>();
                    
                    var seqConfig = hostingContext.Configuration.GetSection("Seq").Get<SeqConfig>();
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.Seq(seqConfig.Url)
                        .WriteTo.Console()
                        .CreateLogger();
                    
                    var youTubeConfig = hostingContext.Configuration.GetSection("YouTube").Get<YouTubeConfig>();

                    services.AddSingleton<YouTubeService>(sp => 
                    {
                        return new YouTubeService(new BaseClientService.Initializer
                        {
                            ApiKey = youTubeConfig.ApiKey
                        });
                    });

                    // Services registration
                    services.AddSingleton<RetryPolicyProvider>();
                    services.AddTransient<IFetcherService, YouTubeFetcherService>(); // Fetches comments
                    services.AddAutoMapper(typeof(MappingConfig));
                    services.AddTransient<ICommentTransformer, CommentThreadToDtoTransformer>(); // Transforms comments to DTO
                    services.AddSingleton<ICommentsService, CommentsService>();

                }).UseSerilog()
                .Build();

            await host.RunAsync();
        }
    }
}