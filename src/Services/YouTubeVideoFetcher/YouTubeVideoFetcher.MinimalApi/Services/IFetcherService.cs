using Google.Apis.YouTube.v3.Data;

namespace YouTubeVideoFetcher.MinimalApi.Services;

public interface IFetcherService
{
    Task<VideoListResponse> GetVideoListByIdAsync(string videoId);
}
