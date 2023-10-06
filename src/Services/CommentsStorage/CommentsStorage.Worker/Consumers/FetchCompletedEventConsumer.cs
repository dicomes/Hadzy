using CommentsStorage.Worker.Contracts.Services;
using IntegrationEventsContracts;
using MassTransit;

namespace CommentsStorage.Worker.Consumers;

public class FetchCompletedEventConsumer : IConsumer<IFetchCompletedEvent>
{
    private readonly IIntegrationService _integrationService;
    private readonly ILogger<FetchCompletedEventConsumer> _logger;
    
    public FetchCompletedEventConsumer(
        IIntegrationService integrationService,
        ILogger<FetchCompletedEventConsumer> logger)
    {
        _integrationService = integrationService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IFetchCompletedEvent> context)
    {
        _logger.LogInformation("{Source}: Received FetchCompletedEvent. EventId: {EventId}.",
            GetType().Name, context.Message.Id);
        
        await _integrationService.UpdateVideoByCompletedEvent(context.Message);
    }
}