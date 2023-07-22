using YouTubeVideoFetcher.MinimalApi.Models;

namespace YouTubeVideoFetcher.MinimalApi.Services;

public interface IVideoHandlerService
{
    APIResponse<T> CreateErrorResponse<T>(string errorMessage);
    Task<IResult> GetVideo(string videoId);
}