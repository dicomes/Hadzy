using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class YouTubeYouTubeCommentsManager : IYouTubeCommentsManager
{
    private readonly IYouTubeFetcherService _youTubeFetcherService;
    private readonly ICommentTransformer _transformer;
    private readonly ILogger<YouTubeYouTubeCommentsManager> _logger;  // Added Logger

    public YouTubeYouTubeCommentsManager(IYouTubeFetcherService youTubeFetcherService, ICommentTransformer transformer, ILogger<YouTubeYouTubeCommentsManager> logger)
    {
        _youTubeFetcherService = youTubeFetcherService;
        _transformer = transformer;
        _logger = logger;
    }

    public async Task<ICommentsFetchedEvent> FetchAndTransformCommentsAsync(FetchSettings fetchSettings)
    {
        var response = await _youTubeFetcherService.FetchAsync(fetchSettings);
        var commentsFetchedEvent = _transformer.Transform(fetchSettings.VideoId, response);
        
        _logger.LogInformation("YouTubeYouTubeCommentsManager: Fetching event completed for VideoId: {VideoId}. PageToken: {PageToken}. Batch size: {CommentsFetchedCount}. Replies count: {RepliesCount}.", fetchSettings.VideoId, fetchSettings.PageToken, commentsFetchedEvent.CommentsCount, commentsFetchedEvent.TotalReplyCount);

        return commentsFetchedEvent;
    }
}
