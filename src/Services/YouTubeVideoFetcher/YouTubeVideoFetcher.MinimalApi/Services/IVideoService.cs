using Google.Apis.YouTube.v3.Data; // Video

namespace YouTubeVideoFetcher.Services;

public interface IVideoService
{
    Task<Video> GetVideoByIdAsync(string videoId);
}
