using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeVideoFetcher.Services;

public class FetcherService : IFetcherService
{
    private readonly YouTubeService _youtubeService;
    private readonly string part = "snippet,statistics";

    public FetcherService(YouTubeService youtubeService)
    {
        _youtubeService = youtubeService;
    }

    public async Task<VideoListResponse> GetVideoListByIdAsync(string videoId)
    {
        var request = _youtubeService.Videos.List(part);
        request.Id = videoId;
        return await request.ExecuteAsync();
    }
}