using Google.Apis.YouTube.v3.Data;
using YouTubeVideoFetcher.MinimalApi.Models.DTO; // Video

namespace YouTubeVideoFetcher.Services;

public interface IVideoService
{
    Task<YouTubeVideoDto> GetVideoByIdAsync(string videoId);
}
