using Microsoft.AspNetCore.Diagnostics;
using YouTubeVideoFetcher.MinimalApi.Services;

namespace YouTubeVideoFetcher.MinimalApi.Extensions;

public static class ExceptionHandlerExtensions
{
    public static WebApplication UseCustomExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature.Error;
                var handlerService = context.RequestServices.GetRequiredService<IExceptionHandlerService>();
                var result = await handlerService.HandleException(exception);
                await result.ExecuteAsync(context);
            });
        });
        return app;
    }
}
