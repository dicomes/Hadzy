using Google;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Enums;
using YouTubeCommentsFetcher.Worker.Exceptions;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class YouTubeFetcherService : IYouTubeFetcherService
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

        try
        {
            return await retryPolicy.ExecuteAsync(async () => 
            {
                _logger.LogInformation("YouTubeFetcherService: Fetching event started for VideoId: {VideoId}. PageToken: {PageToken}", fetchSettings.VideoId, fetchSettings.PageToken);
                var request = _youtubeService.CommentThreads.List(fetchSettings.Properties);
                request.VideoId = fetchSettings.VideoId;
                request.MaxResults = fetchSettings.MaxResults;
                request.PageToken = string.IsNullOrEmpty(fetchSettings.PageToken) ? request.PageToken : fetchSettings.PageToken;
                return await request.ExecuteAsync();
            });
        }
        
        catch (GoogleApiException ex)
        {
            _logger.LogWarning("YouTubeFetcherService: A Google API error occurred while fetching comments for VideoId: {VideoId}", fetchSettings.VideoId);
            throw new YouTubeFetcherServiceException(fetchSettings.VideoId, ex.HttpStatusCode.ToString(), ErrorCategory.GoogleApiError);
        }
        
        catch (HttpRequestException ex)
        {
            _logger.LogWarning("YouTubeFetcherService: An HTTP request error occurred while fetching comments for VideoId: {VideoId}", fetchSettings.VideoId);
            throw new YouTubeFetcherServiceException(fetchSettings.VideoId, ex.Message, ErrorCategory.HttpRequestError);
        }
        
    }
    
}
