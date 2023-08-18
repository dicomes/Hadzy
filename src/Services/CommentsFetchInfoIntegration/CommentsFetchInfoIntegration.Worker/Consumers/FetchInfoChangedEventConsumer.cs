using CommentsFetchInfoIntegration.Worker.Builders;
using CommentsFetchInfoIntegration.Worker.IntegrationEvents;
using CommentsFetchInfoIntegration.Worker.Services.Interfaces;
using MassTransit;
using IntegrationEventsContracts;

namespace CommentsFetchInfoIntegration.Worker.Consumers;

public class FetchInfoChangedEventConsumer : IConsumer<IFetchInfoChangedEvent>
{
    private readonly ILogger<FetchInfoChangedEventConsumer> _logger;
    private readonly IFetchInfoChangedEventHandler _fetchInfoChangedEventHandler;
    private readonly CommentsFetchInfoEventBuilder _eventBuilder;

    public FetchInfoChangedEventConsumer(
        ILogger<FetchInfoChangedEventConsumer> logger,
        IFetchInfoChangedEventHandler fetchInfoChangedEventHandler,
        CommentsFetchInfoEventBuilder eventBuilder)
    {
        _logger = logger;
        _fetchInfoChangedEventHandler = fetchInfoChangedEventHandler;
        _eventBuilder = eventBuilder;
    }

    public async Task Consume(ConsumeContext<IFetchInfoChangedEvent> context)
    {
        FetchInfoChangedEvent fetchInfoChangedEventReceived = _eventBuilder.BuildFromEvent(context.Message);
        _logger.LogInformation("{Source}: Received FetchInfoChangedEvent. Guid: {Guid}. Event data: {EventData}.",
            GetType().Name, fetchInfoChangedEventReceived.Id, fetchInfoChangedEventReceived);

        await _fetchInfoChangedEventHandler.HandeAsync(fetchInfoChangedEventReceived);
    }
}