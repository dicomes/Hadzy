using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class FetchEventsIntegration : IFetchEventsIntegration
{
    private readonly IYouTubeFetcherService _youTubeFetcherService;
    private readonly ICommentMapper _mapper;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<FetchEventsIntegration> _logger;

    public FetchEventsIntegration(
        IYouTubeFetcherService youTubeFetcherService,
        ICommentMapper mapper,
        IEventPublisher eventPublisher,
        ILogger<FetchEventsIntegration> logger)
    {
        _youTubeFetcherService = youTubeFetcherService;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

public async Task FetchAndPublishAsync(string videoId, string pageToken)
{
    int totalCommentsFetched = 0;
    int totalReplies = 0;
    string lastPageToken = string.Empty;
    FetchStatusChangedEvent fetchStatusChangedEvent;
    FetchStatusChangedEventBuilder fetchedStatusChangedEventBuilder = new FetchStatusChangedEventBuilder();;
    
    do
    {
        var fetchSettings = new FetchSettings(videoId, pageToken);

        // Fetch comments
        var response = await _youTubeFetcherService.FetchAsync(fetchSettings);

        // Map response to fetched event
        FetchCompletedEvent fetchCompletedEvent = _mapper.Map(fetchSettings.VideoId, response);
        _logger.LogInformation("FetchEventsIntegration: Fetching batch completed for VideoId: {VideoId}. PageToken: {PageToken}. Batch size: {CommentsFetchedCount}. Replies count: {RepliesCount}.", videoId, pageToken, fetchCompletedEvent.CommentsFetchedCount, fetchCompletedEvent.ReplyCount);
        
        // Publish fetched event
        await _eventPublisher.PublishEvent(fetchCompletedEvent);
        
        // Build fetch status event
        fetchStatusChangedEvent = fetchedStatusChangedEventBuilder
            .WithVideoId(videoId)
            .WithPageToken(pageToken)
            .WithCommentsFetchedCount(fetchCompletedEvent.CommentsFetchedCount)
            .WithReplyCount(fetchCompletedEvent.ReplyCount)
            .WithIsFetching(!string.IsNullOrEmpty(fetchCompletedEvent.NextPageToken))
            .Build();

        // Publish fetch status event
        await _eventPublisher.PublishEvent(fetchStatusChangedEvent);

        // Set PageToken used by the fetcher service
        pageToken = fetchCompletedEvent.NextPageToken;
        
        totalCommentsFetched += fetchCompletedEvent.CommentsFetchedCount;
        totalReplies += fetchCompletedEvent.ReplyCount;

    } while (!string.IsNullOrEmpty(pageToken));
    
    _logger.LogInformation("FetchEventsIntegration: Completed to fetch all batches for VideoId: {VideoId}. Total Comments: {TotalComments}. Total Replies: {TotalReplies}.", videoId, totalCommentsFetched, totalReplies);
}

}
