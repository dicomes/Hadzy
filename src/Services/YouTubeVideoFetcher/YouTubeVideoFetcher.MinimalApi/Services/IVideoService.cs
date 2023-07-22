using YouTubeVideoFetcher.MinimalApi.Models.DTO;

namespace YouTubeVideoFetcher.MinimalApi.Services;

public interface IVideoService
{
    Task<YouTubeVideoDto> GetVideoByIdAsync(string videoId);
}
