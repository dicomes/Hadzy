using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CommentsFetchInfoManager.MinimalApi.Extensions;

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
                var result = handlerService.HandleException(exception);
                await result.ExecuteAsync(context);
            });
        });

        return app;
    }
    
    public static WebApplication ConfigureFetchStatusEndpoints(this WebApplication app)
    {

        app.MapPost("comments-fetch-status-manager/api/v1/comments/fetch/",
                (IFetchInfoHandlerService fetchStatusService, [FromBody] FetchInfoDto? fetchInfoDto) =>
                    fetchStatusService.PostNewFetchInfo(fetchInfoDto))
            .WithName("NewFetchInfo")
            .Accepts<FetchInfoDto>("application/json")
            .Produces<APIResponse<FetchInfoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapGet("comments-fetch-status-manager/api/v1/comments/fetch/status/{videoId}",
                (IFetchInfoHandlerService fetchManagerService, string videoId) =>  
                    fetchManagerService.GetFetchStatusByIdAsync(videoId))
            .WithName("GetFetchStatus")
            .Produces<APIResponse<FetchInfoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
        
        return app;
    }

}