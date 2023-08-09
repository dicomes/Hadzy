using AutoMapper;
using CommentsFetchStatusIntegration.Worker.Builders;
using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
using MassTransit;
using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.Consumers;

public class FetchStatusChangedConsumer : IConsumer<IFetchStatusChangedEvent>
{
    private readonly ILogger<FetchStatusChangedConsumer> _logger;
    private readonly IUpdateFetchStatusEvent _updateFetchStatusEvent;
    private readonly CommentsFetchStatusEventBuilder _eventBuilder;


    public FetchStatusChangedConsumer(
        ILogger<FetchStatusChangedConsumer> logger,
        IUpdateFetchStatusEvent updateFetchStatusEvent,
        CommentsFetchStatusEventBuilder eventBuilder)
    {
        _logger = logger;
        _updateFetchStatusEvent = updateFetchStatusEvent;
        _eventBuilder = eventBuilder;
    }

    public async Task Consume(ConsumeContext<IFetchStatusChangedEvent> context)
    {
        FetchStatusChangedEvent fetchStatusChangedEventReceived = _eventBuilder.BuildFromMessage(context.Message);
        _logger.LogInformation("CommentsFetchStatusIntegration: Received FetchStatusChangedEvent. Guid: {Guid}. Event data: {EventData}.", fetchStatusChangedEventReceived.Id, fetchStatusChangedEventReceived);

        await _updateFetchStatusEvent.UpdateAsync(fetchStatusChangedEventReceived);
    }
}