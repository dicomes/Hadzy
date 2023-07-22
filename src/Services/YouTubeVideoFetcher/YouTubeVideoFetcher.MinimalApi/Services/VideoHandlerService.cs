using YouTubeVideoFetcher.MinimalApi.Models;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;

namespace YouTubeVideoFetcher.MinimalApi.Services;

public class VideoHandlerService : IVideoHandlerService
{
    private readonly IVideoService _videoService;
    private readonly ILogger<VideoHandlerService> _logger;

    public VideoHandlerService(IVideoService videoService, ILogger<VideoHandlerService> logger)
    {
        _videoService = videoService;
        _logger = logger;
    }

    public async Task<IResult> GetVideo(string videoId)
    {
        _logger.LogInformation("GET Request received for VideoId: '{VideoId}'.", videoId);
            
        YouTubeVideoDto videoDto = await _videoService.GetVideoByIdAsync(videoId);
        var response = new APIResponse<YouTubeVideoDto>
        {
            Result = videoDto,
        };

        return Results.Ok(response);
    }

    public APIResponse<T> CreateErrorResponse<T>(string errorMessage)
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