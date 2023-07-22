using YouTubeVideoFetcher.MinimalApi.Models;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;
using YouTubeVideoFetcher.MinimalApi.Services;

namespace YouTubeVideoFetcher.MinimalApi.Endpoints;

public static class WebApplicationExtensions
{
    public static void ConfigureVideoEndpoints(this WebApplication app)
    {
        app.MapGet("video-fetcher/api/v1/video/{videoId}", (IVideoHandlerService videoHandler, string videoId) => videoHandler.GetVideo(videoId))
            .WithName("GetVideo")
            .Produces<APIResponse<YouTubeVideoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
    }

}

