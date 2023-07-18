using Google.Apis.YouTube.v3.Data;

namespace YouTubeVideoFetcher.Services;

public interface IFetcherService
{
    Task<VideoListResponse> GetVideoListByIdAsync(string videoId);
}
