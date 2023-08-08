using MassTransit;
using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.Consumers;

public class CommentsFetchStatusEventConsumer : IConsumer<ICommentsFetchStatusEvent>
{
    private readonly ILogger<CommentsFetchStatusEventConsumer> _logger;

    public CommentsFetchStatusEventConsumer(ILogger<CommentsFetchStatusEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ICommentsFetchStatusEvent> context)
    {
        ICommentsFetchStatusEvent fetchStatus = context.Message;
        _logger.LogInformation("CommentsFetchStatusEventConsumer: Received CommentsFetchStatusEvent. Guid: {Guid}. VideoId: {VideoId}. PageToken: {CommentsCount}.", fetchStatus.Id, fetchStatus.VideoId, fetchStatus.CommentsFetchedCount);
    }
    
}