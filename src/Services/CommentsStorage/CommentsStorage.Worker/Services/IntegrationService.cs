using AutoMapper;
using CommentsStorage.Worker.Models;
using IntegrationEventsContracts;

namespace CommentsStorage.Worker.Services;

public class IntegrationService : IIntegrationService
{
    private readonly ICommentService _commentService;
    private readonly IMapper _mapper;
    
    public IntegrationService(
        ICommentService commentService,
        IMapper mapper)
    {
        _commentService = commentService;
        _mapper = mapper;
    }

    public async Task AddComments(List<IYouTubeCommentDto> commentsDto)
    {
        List<Comment> comments = _mapper.Map<List<Comment>>(commentsDto);
        await _commentService.AddCommentsAsync(comments);
    }
}