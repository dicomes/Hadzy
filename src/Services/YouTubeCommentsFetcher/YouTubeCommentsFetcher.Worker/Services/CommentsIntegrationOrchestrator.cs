using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsIntegrationOrchestrator : ICommentsIntegrationOrchestrator
{
    private readonly IYouTubeFetcherService _youTubeFetcherService;
    private readonly ICommentTransformer _transformer;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<CommentsIntegrationOrchestrator> _logger;

    public CommentsIntegrationOrchestrator(
        IYouTubeFetcherService youTubeFetcherService,
        ICommentTransformer transformer,
        IEventPublisher eventPublisher,
        ILogger<CommentsIntegrationOrchestrator> logger)
    {
        _youTubeFetcherService = youTubeFetcherService;
        _transformer = transformer;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task ProcessCommentsForVideoAsync(string videoId, string initialNextPageToken)
    {
        string nextPageToken = initialNextPageToken;
        int totalCommentsFetched = 0;
        int totalReplies = 0;
        
        do
        {
            var fetchSettings = new FetchSettings(videoId, nextPageToken);

            // Fetch comments
            var response = await _youTubeFetcherService.FetchAsync(fetchSettings);

            // Transform the comments
            CommentsFetchedEvent commentsFetchedEvent = _transformer.Transform(fetchSettings.VideoId, response);

            int commentsFetchedCount = commentsFetchedEvent.CommentsFetchedCount;
            int repliesCount = commentsFetchedEvent.ReplyCount;
            
            _logger.LogInformation("CommentsIntegrationOrchestrator: Fetching batch completed for VideoId: {VideoId}. PageToken: {PageToken}. Batch size: {CommentsFetchedCount}. Replies count: {RepliesCount}.", videoId, nextPageToken, commentsFetchedCount, repliesCount);

            var fetchedStatusEventBuilder = new CommentsFetchedStatusEventBuilder();
            var commentsFetchStatusEvent = fetchedStatusEventBuilder
                .WithVideoId(videoId)
                .WithPageToken(nextPageToken)
                .WithCommentsFetchedCount(commentsFetchedCount)
                .WithReplyCount(repliesCount)
                .Build();

            // Publish fetched event
            await _eventPublisher.PublishEvent(commentsFetchedEvent);
            
            // Publish fetch status event
            await _eventPublisher.PublishEvent(commentsFetchStatusEvent);
            
            totalCommentsFetched += commentsFetchedCount;
            totalReplies += repliesCount;

            // Set PageToken used by the fetcher service
            nextPageToken = commentsFetchedEvent.PageToken; // set the next page token for the next iteration

        } while (!string.IsNullOrEmpty(nextPageToken));

        _logger.LogInformation("CommentsIntegrationOrchestrator: Completed to fetch all batches for VideoId: {VideoId}. Total Comments: {TotalComments}. Total Replies: {TotalReplies}.", videoId, totalCommentsFetched, totalReplies);
    }
}
