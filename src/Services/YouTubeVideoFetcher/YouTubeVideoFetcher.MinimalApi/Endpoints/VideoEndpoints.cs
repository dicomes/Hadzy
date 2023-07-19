using YouTubeVideoFetcher.MinimalApi.Models;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;
using YouTubeVideoFetcher.Services;

namespace YouTubeVideoFetcher.MinimalApi.Endpoints;

public static class VideoEndpoints
{
    public static void ConfigureVideoEndpoints(this WebApplication app)
    {
        app.MapGet("video-fetcher/api/v1/video/{videoId}", GetVideo)
            .WithName("GetVideo")
            .Produces<APIResponse<YouTubeVideoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    private async static Task<IResult> GetVideo(IVideoService _videoService, string videoId)
    {
        YouTubeVideoDto videoDto = await _videoService.GetVideoByIdAsync(videoId);

        var response = new APIResponse<YouTubeVideoDto>
        {
            Result = videoDto,
        };

        return Results.Ok(response);
    }
    
    public static APIResponse<T> CreateErrorResponse<T>(string errorMessage)
    {
        var response = new APIResponse<T>()
        {
            Result = default
        };

        if (!string.IsNullOrEmpty(errorMessage))
        {
            response.ErrorMessages.Add(errorMessage);
        }

        return response;
    }


}
