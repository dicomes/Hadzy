using AutoMapper;
using CommentsFetchStatusIntegration.Worker.Builders;
using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
using MassTransit;
using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.Consumers;

public class CommentsFetchStatusEventConsumer : IConsumer<ICommentsFetchStatusEvent>
{
    private readonly ILogger<CommentsFetchStatusEventConsumer> _logger;
    private readonly ICommentsFetchStatusProcessor _commentsFetchStatusProcessor;
    private readonly CommentsFetchStatusEventBuilder _eventBuilder;


    public CommentsFetchStatusEventConsumer(
        ILogger<CommentsFetchStatusEventConsumer> logger,
        ICommentsFetchStatusProcessor commentsFetchStatusProcessor,
        CommentsFetchStatusEventBuilder eventBuilder)
    {
        _logger = logger;
        _commentsFetchStatusProcessor = commentsFetchStatusProcessor;
        _eventBuilder = eventBuilder;
    }

    public async Task Consume(ConsumeContext<ICommentsFetchStatusEvent> context)
    {
        CommentsFetchStatusEvent fetchStatusEventReceived = _eventBuilder.BuildFromMessage(context.Message);
        _logger.LogInformation("CommentsFetchStatusEventConsumer: Received CommentsFetchStatusEvent. Guid: {Guid}. Event data: {EventData}.", fetchStatusEventReceived.Id, fetchStatusEventReceived);

        await _commentsFetchStatusProcessor.ProcessVideoCommentsStatusAsync(fetchStatusEventReceived);
    }
}