using Google;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Builders;
using YouTubeCommentsFetcher.Worker.Contracts;
using YouTubeCommentsFetcher.Worker.Enums;
using YouTubeCommentsFetcher.Worker.Exceptions;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services;

public class YouTubeFetcher : IYouTubeFetcher
{
    private readonly string _properties = "snippet";
    private readonly int _maxResults = 100;
    private readonly YouTubeService _youtubeService;
    private readonly RetryPolicyProvider _retryPolicyProvider;
    private readonly ILogger<YouTubeFetcher> _logger;
    
    public YouTubeFetcher(YouTubeService youtubeService, RetryPolicyProvider retryPolicyProvider, ILogger<YouTubeFetcher> logger)
    {
        _youtubeService = youtubeService;
        _retryPolicyProvider = retryPolicyProvider;
        _logger = logger;
    }

    public async Task<CommentThreadListResponse> FetchAsync(FetchParams fetchParams)
    {
        var retryPolicy = _retryPolicyProvider.GetYouTubeApiRetryPolicy();

        try
        {
            return await retryPolicy.ExecuteAsync(async () => 
            {
                _logger.LogInformation("{Source}: Fetching event started for VideoId: {VideoId}. NextPageToken: {NextPageToken}",GetType().Name, fetchParams.VideoId, fetchParams.PageToken);
                var request = new YouTubeRequestBuilder(_youtubeService, _properties)
                    .SetMaxResults(_maxResults)
                    .SetVideoId(fetchParams.VideoId)
                    .SetPageToken(fetchParams.PageToken)
                    .Build();
                return await request.ExecuteAsync();
            });
        }
        
        catch (GoogleApiException ex)
        {
            _logger.LogWarning("{Source}: A Google API error occurred while fetching comments for VideoId: {VideoId}",GetType().Name, fetchParams.VideoId);
            throw new YouTubeFetcherServiceException(fetchParams.VideoId, ex.HttpStatusCode.ToString(), ErrorCategory.GoogleApiError);
        }
        
        catch (HttpRequestException ex)
        {
            _logger.LogWarning("{Source}: An HTTP request error occurred while fetching comments for VideoId: {VideoId}", GetType().Name, fetchParams.VideoId);
            throw new YouTubeFetcherServiceException(fetchParams.VideoId, ex.Message, ErrorCategory.HttpRequestError);
        }
        
    }
    
}
