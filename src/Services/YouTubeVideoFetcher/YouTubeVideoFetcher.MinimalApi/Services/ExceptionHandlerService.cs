using System.Net;
using Google;
using YouTubeVideoFetcher.MinimalApi.Exceptions;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;

namespace YouTubeVideoFetcher.MinimalApi.Services;

public class ExceptionHandlerService : IExceptionHandlerService
{
    private readonly IVideoHandlerService _videoHandlerService;
    public ExceptionHandlerService(IVideoHandlerService videoHandlerService)
    {
        _videoHandlerService = videoHandlerService;
    }
    public async Task<IResult> HandleException(Exception exception)
    {
        if (exception is VideoNotFoundException)
        {
            var errorResponse = _videoHandlerService.CreateErrorResponse<YouTubeVideoDto>(exception.Message);
            return Results.NotFound(errorResponse);
        }
        
        if (exception is VideoBadRequestException)
        {
            var errorResponse = _videoHandlerService.CreateErrorResponse<YouTubeVideoDto>(exception.Message);
            return Results.BadRequest(errorResponse);
        }
        
        if (exception is VideoAccessForbiddenException)
        {
            return Results.Forbid();
        }
        
        if (exception is GoogleApiException)
        {
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }
        
        return Results.StatusCode((int)HttpStatusCode.InternalServerError);
    }
}