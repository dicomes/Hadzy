using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsIntegrationOrchestrator : ICommentsIntegrationOrchestrator
{
    private readonly IYouTubeFetcherService _youTubeFetcherService;
    private readonly ICommentMapper _mapper;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<CommentsIntegrationOrchestrator> _logger;

    public CommentsIntegrationOrchestrator(
        IYouTubeFetcherService youTubeFetcherService,
        ICommentMapper mapper,
        IEventPublisher eventPublisher,
        ILogger<CommentsIntegrationOrchestrator> logger)
    {
        _youTubeFetcherService = youTubeFetcherService;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

public async Task ProcessCommentsForVideoAsync(string videoId, string pageToken)
{
    int totalCommentsFetched = 0;
    int totalReplies = 0;
    CommentsFetchStatusEvent commentsFetchStatusEvent;
    CommentsFetchedStatusEventBuilder fetchedStatusEventBuilder = new CommentsFetchedStatusEventBuilder();;

    do
    {
        var fetchSettings = new FetchSettings(videoId, pageToken);

        // Fetch comments
        var response = await _youTubeFetcherService.FetchAsync(fetchSettings);

        // Transform the comments
        CommentsFetchedEvent commentsFetchedEvent = _mapper.Map(fetchSettings.VideoId, response);

        int commentsFetchedCount = commentsFetchedEvent.CommentsFetchedCount;
        int repliesCount = commentsFetchedEvent.ReplyCount;

        _logger.LogInformation("CommentsIntegrationOrchestrator: Fetching batch completed for VideoId: {VideoId}. PageToken: {PageToken}. Batch size: {CommentsFetchedCount}. Replies count: {RepliesCount}.", videoId, pageToken, commentsFetchedCount, repliesCount);

        commentsFetchStatusEvent = fetchedStatusEventBuilder
            .WithVideoId(videoId)
            .WithPageToken(commentsFetchedEvent.PageToken)
            .WithCommentsFetchedCount(commentsFetchedCount)
            .WithReplyCount(repliesCount)
            .WithIsFetching(true)
            .Build();

        // Publish fetched event
        await _eventPublisher.PublishEvent(commentsFetchedEvent);

        // Publish fetch status event with IsFetching = true
        await _eventPublisher.PublishEvent(commentsFetchStatusEvent);

        totalCommentsFetched += commentsFetchedCount;
        totalReplies += repliesCount;

        // Set PageToken used by the fetcher service
        pageToken = commentsFetchedEvent.PageToken; // set the next page token for the next iteration

    } while (!string.IsNullOrEmpty(pageToken));

    // After fetching, pass the event to denote the fetching is complete
    commentsFetchStatusEvent = fetchedStatusEventBuilder
        .WithVideoId(videoId)
        .WithIsFetching(false)
        .Build();

    // Publish fetch status event with IsFetching = false
    await _eventPublisher.PublishEvent(commentsFetchStatusEvent);

    _logger.LogInformation("CommentsIntegrationOrchestrator: Completed to fetch all batches for VideoId: {VideoId}. Total Comments: {TotalComments}. Total Replies: {TotalReplies}.", videoId, totalCommentsFetched, totalReplies);
}

}
