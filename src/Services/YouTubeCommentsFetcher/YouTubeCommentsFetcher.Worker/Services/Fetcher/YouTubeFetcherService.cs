using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Models;
using ILogger = Serilog.ILogger;

namespace YouTubeCommentsFetcher.Worker.Services.Fetcher;

public class YouTubeFetcherService : IFetcherService
{
    private readonly YouTubeService _youtubeService;
    private readonly RetryPolicyProvider _retryPolicyProvider;
    private readonly ILogger<YouTubeFetcherService> _logger;
    
    public YouTubeFetcherService(YouTubeService youtubeService, RetryPolicyProvider retryPolicyProvider, ILogger<YouTubeFetcherService> logger)
    {
        _youtubeService = youtubeService;
        _retryPolicyProvider = retryPolicyProvider;
        _logger = logger;
    }

    public async Task<CommentThreadListResponse> FetchAsync(FetchSettings fetchSettings)
    {
        var retryPolicy = _retryPolicyProvider.GetYouTubeApiRetryPolicy();
        
        return await retryPolicy.ExecuteAsync(async () => 
        {
            _logger.LogInformation("Fetching batch started for VideoId: {VideoId}. PageToken: {PageToken}", fetchSettings.VideoId, fetchSettings.PageToken);
            var request = _youtubeService.CommentThreads.List(fetchSettings.Properties);
            request.VideoId = fetchSettings.VideoId;
            request.MaxResults = fetchSettings.MaxResults;
            request.PageToken = string.IsNullOrEmpty(fetchSettings.PageToken) ? request.PageToken : fetchSettings.PageToken;
            return await request.ExecuteAsync();
        });
    }
}
