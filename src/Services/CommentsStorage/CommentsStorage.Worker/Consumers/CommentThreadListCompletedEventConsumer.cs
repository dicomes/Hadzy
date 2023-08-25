using AutoMapper;
using CommentsStorage.Worker.Model;
using IntegrationEventsContracts;
using MassTransit;

namespace CommentsStorage.Worker.Consumers;

public class CommentThreadListCompletedEventConsumer : IConsumer<ICommentThreadListCompletedEvent>
{
    private readonly ILogger<CommentThreadListCompletedEventConsumer> _logger;
    private readonly IMapper _mapper;
    
    public CommentThreadListCompletedEventConsumer(
        ILogger<CommentThreadListCompletedEventConsumer> logger,
        IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICommentThreadListCompletedEvent> context)
    {
        List<Comment> comments = _mapper.Map<List<Comment>>(context.Message.YouTubeCommentsList);
        if (comments.Count == 0)
        {
            return;
        }
        
        foreach (Comment comment in comments)
        {
            _logger.LogInformation("CommentThreadListCompletedEventConsumer. Consumed comment: {Comment}", comment);
        }
    }
}