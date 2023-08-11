using CommentsFetchStatus.MinimalApi.Models;
using CommentsFetchStatus.MinimalApi.Models.DTO;
using CommentsFetchStatus.MinimalApi.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

namespace CommentsFetchStatus.MinimalApi.Extensions;

public static class WebApplicatoinExtensions
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
    
    public static WebApplication ConfigureFetchStatusEndpoints(this WebApplication app)
    {
        app.MapGet("comments-fetch-status-manager/api/v1/video/{videoId}",
                (IFetchStatusService fetchStatusService, string videoId) =>
                    fetchStatusService.GetStatusByIdAsync(videoId))
            .WithName("GetVideo")
            .Produces<APIResponse<CommentsFetchStatusDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        
        return app;
    }

}