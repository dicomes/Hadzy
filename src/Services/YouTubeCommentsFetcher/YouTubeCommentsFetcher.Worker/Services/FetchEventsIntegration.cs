using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class FetchEventsIntegration : IFetchEventsIntegration
{
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<FetchEventsIntegration> _logger;
    private readonly ICommentsIterator _commentsIterator;

    public FetchEventsIntegration(
        IEventPublisher eventPublisher,
        ILogger<FetchEventsIntegration> logger,
        ICommentsIterator commentsIterator)
    {
        _eventPublisher = eventPublisher;
        _logger = logger;
        _commentsIterator = commentsIterator;
    }

public async Task FetchAndPublishAsync(string videoId, string pageToken)
{
    int totalCommentsFetched = 0;
    int totalReplies = 0;
    FetchStatusChangedEventBuilder fetchedStatusChangedEventBuilder = new FetchStatusChangedEventBuilder();;
    
    var fetchSettings = new FetchSettings(videoId, pageToken);

    do
    {
        string eventPageToken = fetchSettings.PageToken;
        FetchCompletedEvent fetchCompletedEvent  = await _commentsIterator.Next(fetchSettings);
        
        // Set NextPageToken for next iteration
        fetchSettings.PageToken = fetchCompletedEvent.NextPageToken;
        _logger.LogInformation("FetchEventsIntegration: Fetching batch completed for VideoId: {VideoId}. PageToken: {PageToken}. Batch size: {CommentsFetchedCount}. Replies count: {RepliesCount}.", videoId, pageToken, fetchCompletedEvent.CommentsFetchedCount, fetchCompletedEvent.ReplyCount);
        
        // Publish fetched event
        await _eventPublisher.PublishEvent(fetchCompletedEvent);
        
        // Build fetch status event
        var fetchStatusChangedEvent = fetchedStatusChangedEventBuilder
            .WithVideoId(videoId)
            .WithPageToken(eventPageToken)
            .WithCommentsFetchedCount(fetchCompletedEvent.CommentsFetchedCount)
            .WithReplyCount(fetchCompletedEvent.ReplyCount)
            .WithIsFetching(!string.IsNullOrEmpty(fetchCompletedEvent.NextPageToken))
            .Build();

        // Publish fetch status event
        await _eventPublisher.PublishEvent(fetchStatusChangedEvent);
        totalCommentsFetched += fetchCompletedEvent.CommentsFetchedCount;
        totalReplies += fetchCompletedEvent.ReplyCount;
    } while (_commentsIterator.HasNext());
    
    _logger.LogInformation("FetchEventsIntegration: Completed to fetch all batches for VideoId: {VideoId}. Total Comments: {TotalComments}. Total Replies: {TotalReplies}.", videoId, totalCommentsFetched, totalReplies);
}

}
