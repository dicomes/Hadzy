using MassTransit;
using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsPublishingService : ICommentsPublishingService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CommentsPublishingService> _logger;

    public CommentsPublishingService(IPublishEndpoint publishEndpoint, ILogger<CommentsPublishingService> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishCommentsFetchedEventAsync(ICommentsFetchedEvent commentsFetchedEvent)
    {
        await _publishEndpoint.Publish<ICommentsFetchedEvent>(commentsFetchedEvent);
        _logger.LogInformation("CommentsPublishingService: Published fetched CommentsFetchedEvent completed for VideoId: {VideoId}. PageToken: {PageToken}.", commentsFetchedEvent.VideoId, commentsFetchedEvent.PageToken);
    }
}