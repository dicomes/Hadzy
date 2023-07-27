using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeVideoFetcher.MinimalApi.Configurations;
using YouTubeVideoFetcher.MinimalApi.Services;

namespace YouTubeVideoFetcher.MinimalApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddYouTubeService(this IServiceCollection services, YouTubeConfig youTubeConfig)
        {
            services.AddSingleton<YouTubeService>(provider => 
                new YouTubeService(new BaseClientService.Initializer { ApiKey = youTubeConfig.ApiKey }));
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddSingleton<IFetcherService, FetcherService>();
            services.AddSingleton<IVideoService, VideoService>();
            services.AddSingleton<IVideoHandlerService, VideoHandlerService>();
            services.AddSingleton<IExceptionHandlerService, ExceptionHandlerService>();
            return services;
        }
    }
}