using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeCommentsFetcher.Worker.Configurations;
using YouTubeCommentsFetcher.Worker.Services;

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

                    var youTubeConfig = hostingContext.Configuration.GetSection("YouTube").Get<YouTubeConfig>();

                    services.AddSingleton<YouTubeService>(sp => 
                    {
                        return new YouTubeService(new BaseClientService.Initializer
                        {
                            ApiKey = youTubeConfig.ApiKey
                        });
                    });

                    services.AddSingleton<IFetcherService, FetcherService>();
                    services.AddAutoMapper(typeof(MappingConfig));
                    services.AddSingleton<ICommentsService, CommentsService>();
                    
                })
                .Build();

            await host.RunAsync();
        }
    }
}