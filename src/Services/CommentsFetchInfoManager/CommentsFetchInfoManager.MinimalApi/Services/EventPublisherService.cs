using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using MassTransit;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class EventPublisherService : IEventPublisherService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<EventPublisherService> _logger;

    public EventPublisherService(IPublishEndpoint publishEndpoint, ILogger<EventPublisherService> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishEvent<TEvent>(TEvent eventObject)
    {
        await _publishEndpoint.Publish(eventObject);
        LogEvent(eventObject);
    }

    private void LogEvent<TEvent>(TEvent eventObject)
    {
        _logger.LogInformation("CommentsFetchInfo.EventsPublisher: Event of type {EventType} successfully published. {EventData}", typeof(TEvent).Name, eventObject);
    }
}