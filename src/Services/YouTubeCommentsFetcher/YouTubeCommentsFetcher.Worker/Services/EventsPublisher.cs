using MassTransit;
using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class EventsPublisher : ICommentsFetchedEventPublisher, ICommentsFetchStatusEventPublisher, IErrorEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<EventsPublisher> _logger;

    public EventsPublisher(IPublishEndpoint publishEndpoint, ILogger<EventsPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishFetchedEvent(ICommentsFetchedEvent commentsFetchedEvent)
    {
        await _publishEndpoint.Publish<ICommentsFetchedEvent>(commentsFetchedEvent);
        _logger.LogInformation("CommentsFetchedEventPublisher: Event <ICommentsFetchedEvent> successfully published for VideoId: {VideoId}. PageToken: {PageToken}.", commentsFetchedEvent.VideoId, commentsFetchedEvent.PageToken);
    }

    public async Task PublishFetchStatusEvent(ICommentsFetchStatusEvent commentsFetchStatusEvent)
    {
        await _publishEndpoint.Publish<ICommentsFetchStatusEvent>(commentsFetchStatusEvent);
        _logger.LogInformation("CommentsFetchedEventPublisher: Event <ICommentsFetchStatusEvent> successfully published for VideoId: {VideoId}. PageToken: {PageToken}. Comments count {CommentsFetchedCount}. Reply count: {ReplyCount}", commentsFetchStatusEvent.VideoId, commentsFetchStatusEvent.PageToken, commentsFetchStatusEvent.CommentsFetchedCount, commentsFetchStatusEvent.ReplyCount);
    }

    public async Task PublishErrorEvent(IFetcherErrorEvent errorEvent)
    {
        await _publishEndpoint.Publish<IFetcherErrorEvent>(errorEvent);
        _logger.LogInformation("EventsPublisher: Error event <IFetcherErrorEvent> successfully published for VideoId: {VideoId}. Message: {Message}. ErrorCategory: {ErrorCategory}.", errorEvent.VideoId, errorEvent.Message, errorEvent.ErrorCategory);
    }
}
