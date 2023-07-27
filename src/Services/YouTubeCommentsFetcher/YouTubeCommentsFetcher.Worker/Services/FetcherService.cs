using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeCommentsFetcher.Worker.Services;

public class FetcherService : IFetcherService
{
    private readonly YouTubeService _youtubeService;
    private readonly string properties = "snippet";
    
    public FetcherService(YouTubeService youtubeService)
    {
        _youtubeService = youtubeService;
    }

    public async Task<CommentThreadListResponse> GetCommentThreadList(string videoId, int maxResults)
    {
        var request = _youtubeService.CommentThreads.List(properties);
        request.MaxResults = maxResults;
        request.VideoId = videoId;
        return await request.ExecuteAsync();
    }
}