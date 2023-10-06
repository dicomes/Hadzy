using CommentsStorage.Worker.Contracts.Services;
using IntegrationEventsContracts;
using MassTransit;

namespace CommentsStorage.Worker.Consumers;

public class FetchStartedEventConsumer : IConsumer<IFetchStartedEvent>
{
    private readonly IIntegrationService _integrationService;
    private readonly ILogger<FetchStartedEventConsumer> _logger;
    
    public FetchStartedEventConsumer(
        IIntegrationService integrationService,
        ILogger<FetchStartedEventConsumer> logger)
    {
        _integrationService = integrationService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IFetchStartedEvent> context)
    {
        _logger.LogInformation("{Source}: Received FetchStartedEvent. EventId: {EventId}.",
            GetType().Name, context.Message.Id);
        
        await _integrationService.AddVideoByStartedEvent(context.Message);
    }
}