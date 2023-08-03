using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsIntegrationOrchestrator : ICommentsIntegrationOrchestrator
{
    private readonly IYouTubeCommentsManager _youTubeCommentsManager;
    private readonly ICommentsPublishingService _publishingService;
    private readonly ILogger<CommentsIntegrationOrchestrator> _logger;

    public CommentsIntegrationOrchestrator(
        IYouTubeCommentsManager youTubeCommentsManager, 
        ICommentsPublishingService publishingService,
        ILogger<CommentsIntegrationOrchestrator> logger)
    {
        _youTubeCommentsManager = youTubeCommentsManager;
        _publishingService = publishingService;
        _logger = logger;
    }

    public async Task ProcessCommentsForVideoAsync(string videoId, string nextPageToken)
    {
        var fetchSettings = new FetchSettings(videoId, nextPageToken);

        // Retrieve
        var commentsFetchedEvent = await _youTubeCommentsManager.FetchAndTransformCommentsAsync(fetchSettings);

        // Publish
        await _publishingService.PublishCommentsFetchedEventAsync(commentsFetchedEvent);

        _logger.LogInformation("CommentsIntegrationOrchestrator: Comments processing completed for VideoId: {VideoId}.", videoId);
    }
}