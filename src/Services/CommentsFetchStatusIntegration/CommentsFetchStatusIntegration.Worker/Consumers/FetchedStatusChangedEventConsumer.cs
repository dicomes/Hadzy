using AutoMapper;
using CommentsFetchStatusIntegration.Worker.Builders;
using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
using MassTransit;
using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.Consumers;

public class FetchedStatusChangedEventConsumer : IConsumer<IFetchStatusChangedEvent>
{
    private readonly ILogger<FetchedStatusChangedEventConsumer> _logger;
    private readonly IFetchedStatusChangedEventHandler _fetchedStatusChangedEventHandler;
    private readonly CommentsFetchStatusEventBuilder _eventBuilder;

    public FetchedStatusChangedEventConsumer(
        ILogger<FetchedStatusChangedEventConsumer> logger,
        IFetchedStatusChangedEventHandler fetchedStatusChangedEventHandler,
        CommentsFetchStatusEventBuilder eventBuilder)
    {
        _logger = logger;
        _fetchedStatusChangedEventHandler = fetchedStatusChangedEventHandler;
        _eventBuilder = eventBuilder;
    }

    public async Task Consume(ConsumeContext<IFetchStatusChangedEvent> context)
    {
        FetchStatusChangedEvent fetchStatusChangedEventReceived = _eventBuilder.BuildFromEvent(context.Message);
        _logger.LogInformation("CommentsFetchStatusIntegration: Received FetchStatusChangedEvent. Guid: {Guid}. Event data: {EventData}.", fetchStatusChangedEventReceived.Id, fetchStatusChangedEventReceived);

        await _fetchedStatusChangedEventHandler.HandeAsync(fetchStatusChangedEventReceived);
    }
}