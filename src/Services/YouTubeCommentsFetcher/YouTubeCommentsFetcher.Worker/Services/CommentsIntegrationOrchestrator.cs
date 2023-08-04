using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsIntegrationOrchestrator : ICommentsIntegrationOrchestrator
{
    private readonly IYouTubeFetcherService _youTubeFetcherService;
    private readonly ICommentTransformer _transformer;
    private readonly ICommentsPublishingService _publishingService;
    private readonly ILogger<CommentsIntegrationOrchestrator> _logger;

    public CommentsIntegrationOrchestrator(
        IYouTubeFetcherService youTubeFetcherService,
        ICommentTransformer transformer,
        ICommentsPublishingService publishingService,
        ILogger<CommentsIntegrationOrchestrator> logger)
    {
        _youTubeFetcherService = youTubeFetcherService;
        _transformer = transformer;
        _publishingService = publishingService;
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
            var commentsFetchedEvent = _transformer.Transform(fetchSettings.VideoId, response);

            int commentsFetchedCount = commentsFetchedEvent.CommentsFetchedCount;
            int repliesCount = commentsFetchedEvent.ReplyCount;
            
            _logger.LogInformation("CommentsIntegrationOrchestrator: Fetching batch completed for VideoId: {VideoId}. PageToken: {PageToken}. Batch size: {CommentsFetchedCount}. Replies count: {RepliesCount}.", videoId, nextPageToken, commentsFetchedCount, repliesCount);

            totalCommentsFetched += commentsFetchedCount;
            totalReplies += repliesCount;

            // Publish
            await _publishingService.PublishCommentsFetchedEventAsync(commentsFetchedEvent);
            
            // Set PageToken for next batch
            nextPageToken = commentsFetchedEvent.PageToken; // set the next page token for the next iteration

        } while (!string.IsNullOrEmpty(nextPageToken));

        _logger.LogInformation("CommentsIntegrationOrchestrator: Completed to fetch all batches for VideoId: {VideoId}. Total Comments: {TotalComments}. Total Replies: {TotalReplies}.", videoId, totalCommentsFetched, totalReplies);
    }
}
