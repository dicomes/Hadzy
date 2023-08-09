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
    FetchStatusChangedEvent fetchStatusChangedEvent;
    FetchStatusChangedEventBuilder fetchedStatusChangedEventBuilder = new FetchStatusChangedEventBuilder();;

    do
    {
        var fetchSettings = new FetchSettings(videoId, pageToken);

        // Fetch comments
        var response = await _youTubeFetcherService.FetchAsync(fetchSettings);

        // Transform the comments
        FetchCompletedEvent fetchCompletedEvent = _mapper.Map(fetchSettings.VideoId, response);

        int commentsFetchedCount = fetchCompletedEvent.CommentsFetchedCount;
        int repliesCount = fetchCompletedEvent.ReplyCount;

        _logger.LogInformation("FetchEventsIntegration: Fetching batch completed for VideoId: {VideoId}. PageToken: {PageToken}. Batch size: {CommentsFetchedCount}. Replies count: {RepliesCount}.", videoId, pageToken, commentsFetchedCount, repliesCount);

        fetchStatusChangedEvent = fetchedStatusChangedEventBuilder
            .WithVideoId(videoId)
            .WithPageToken(fetchCompletedEvent.PageToken)
            .WithCommentsFetchedCount(commentsFetchedCount)
            .WithReplyCount(repliesCount)
            .WithIsFetching(true)
            .Build();

        // Publish fetched event
        await _eventPublisher.PublishEvent(fetchCompletedEvent);

        // Publish fetch status event with IsFetching = true
        await _eventPublisher.PublishEvent(fetchStatusChangedEvent);

        totalCommentsFetched += commentsFetchedCount;
        totalReplies += repliesCount;

        // Set PageToken used by the fetcher service
        pageToken = fetchCompletedEvent.PageToken; // set the next page token for the next iteration

    } while (!string.IsNullOrEmpty(pageToken));

    // After fetching, pass the event to denote the fetching is complete
    fetchStatusChangedEvent = fetchedStatusChangedEventBuilder
        .WithVideoId(videoId)
        .WithIsFetching(false)
        .Build();

    // Publish fetch status event with IsFetching = false
    await _eventPublisher.PublishEvent(fetchStatusChangedEvent);

    _logger.LogInformation("FetchEventsIntegration: Completed to fetch all batches for VideoId: {VideoId}. Total Comments: {TotalComments}. Total Replies: {TotalReplies}.", videoId, totalCommentsFetched, totalReplies);
}

}
