using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class IntegrationEventsManager : IIntegrationEventsManager
{
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<IntegrationEventsManager> _logger;
    private readonly ICommentsIterator _commentsIterator;

    public IntegrationEventsManager(
        IEventPublisher eventPublisher,
        ILogger<IntegrationEventsManager> logger,
        ICommentsIterator commentsIterator)
    {
        _eventPublisher = eventPublisher;
        _logger = logger;
        _commentsIterator = commentsIterator;
    }

    public async Task FetchCommentsAndPublishFetchedEventsAsync(string videoId, string pageToken)
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
            _logger.LogInformation("IntegrationEventsManager: Fetching batch completed for VideoId: {VideoId}. PageToken: {PageToken}. Batch size: {CommentsCount}. Replies count: {RepliesCount}.", videoId, pageToken, fetchCompletedEvent.CommentsFetchedCount, fetchCompletedEvent.ReplyCount);
            
            // Publish fetched event
            await _eventPublisher.PublishEvent(fetchCompletedEvent);
            
            // Build fetch status event
            var fetchStatusChangedEvent = fetchedStatusChangedEventBuilder
                .WithVideoId(videoId)
                .WithPageToken(eventPageToken)
                .WithCommentsFetchedCount(fetchCompletedEvent.CommentsFetchedCount)
                .WithReplyCount(fetchCompletedEvent.ReplyCount)
                .WithStatus(string.IsNullOrEmpty(fetchCompletedEvent.NextPageToken) ? 
                    FetchStatus.Done.ToString() : FetchStatus.InProgress.ToString())
                .Build();

            // Publish fetch status event
            await _eventPublisher.PublishEvent(fetchStatusChangedEvent);
            totalCommentsFetched += fetchCompletedEvent.CommentsFetchedCount;
            totalReplies += fetchCompletedEvent.ReplyCount;
        } while (_commentsIterator.HasNext());
        
        _logger.LogInformation("IntegrationEventsManager: Completed to fetch all batches for VideoId: {VideoId}. Total Comments: {TotalComments}. Total Replies: {TotalReplies}.", videoId, totalCommentsFetched, totalReplies);
    }
}
