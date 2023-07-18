using System.Net;
using Google;
using YouTubeVideoFetcher.MinimalApi.Endpoints;
using YouTubeVideoFetcher.MinimalApi.Exceptions;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;

namespace YouTubeVideoFetcher.Services;

public class ExceptionHandlerService : IExceptionHandlerService
{
    public async Task<IResult> HandleException(Exception exception)
    {
        if (exception is VideoNotFoundException)
        {
            var errorResponse = VideoEndpoints.CreateErrorResponse<YouTubeVideoDto>(exception.Message);
            return Results.NotFound(errorResponse);
        }
        else if (exception is VideoBadRequestException)
        {
            var errorResponse = VideoEndpoints.CreateErrorResponse<YouTubeVideoDto>(exception.Message);
            return Results.BadRequest(errorResponse);
        }
        else if (exception is VideoAccessForbiddenException)
        {
            return Results.Forbid();
        }
        else if (exception is GoogleApiException)
        {
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }
        else
        {
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}