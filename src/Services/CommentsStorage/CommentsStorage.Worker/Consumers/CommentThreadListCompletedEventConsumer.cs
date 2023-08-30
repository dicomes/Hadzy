using AutoMapper;
using CommentsStorage.Worker.Services;
using IntegrationEventsContracts;
using MassTransit;

namespace CommentsStorage.Worker.Consumers;

public class CommentThreadListCompletedEventConsumer : IConsumer<ICommentThreadListCompletedEvent>
{
    private readonly IIntegrationService _integrationService;
    private readonly ILogger<CommentThreadListCompletedEventConsumer> _logger;
    
    public CommentThreadListCompletedEventConsumer(
        IIntegrationService integrationService,
        ILogger<CommentThreadListCompletedEventConsumer> logger)
    {
        _integrationService = integrationService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ICommentThreadListCompletedEvent> context)
    {
        _logger.LogInformation("{Source}: Received CommentThreadListCompletedEvent. EventId: {EventId}.",
            GetType().Name, context.Message.Id);
        
        await _integrationService.AddComments(context.Message.YouTubeCommentsList);
        
        _logger.LogInformation("{Source}: Comments added to DB. EventId: {EventId}. VideoId: {VideoId}",
            GetType().Name, context.Message.Id, context.Message.VideoId);
    }
}