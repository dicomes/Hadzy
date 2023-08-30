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

    public FetchInfoChangedEventConsumer(
        ILogger<FetchInfoChangedEventConsumer> logger,
        IFetchInfoChangedEventHandler fetchInfoChangedEventHandler)
    {
        _logger = logger;
        _fetchInfoChangedEventHandler = fetchInfoChangedEventHandler;
    }

    public async Task Consume(ConsumeContext<IFetchInfoChangedEvent> context)
    {
        
        _logger.LogInformation("{Source}: Received FetchInfoChangedEvent. Event Id: {EventId}. Event data: {EventData}.",
            GetType().Name, context.Message.Id, context.Message);

        await _fetchInfoChangedEventHandler.HandeAsync(context.Message);
    }
}