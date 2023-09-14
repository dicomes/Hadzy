using AutoMapper;
using CommentsStorage.Worker.Contracts.Services;
using CommentsStorage.Worker.Models;
using IntegrationEventsContracts;

namespace CommentsStorage.Worker.Services;

public class IntegrationService : IIntegrationService
{
    private readonly ICommentService _commentService;
    private readonly IVideoService _videoService;
    private readonly IMapper _mapper;
    
    public IntegrationService(
        ICommentService commentService,
        IMapper mapper, IVideoService videoService)
    {
        _commentService = commentService;
        _mapper = mapper;
        _videoService = videoService;
    }

    public async Task AddComments(List<IYouTubeCommentDto> commentsDto)
    {
        List<Comment> comments = _mapper.Map<List<Comment>>(commentsDto);
        await _commentService.AddCommentsAsync(comments);
    }

    public async Task HandleVideo(ICommentThreadListCompletedEvent commentThreadEvent)
    {
        var video = BuildVideoFromEvent(commentThreadEvent);
        await AddOrUpdateVideo(video);
    }
    
    private async Task AddOrUpdateVideo(Video video)
    {
        await _videoService.AddOrUpdateAsync(video);
    }
    
    private Video BuildVideoFromEvent(ICommentThreadListCompletedEvent commentThreadEvent)
    {
        Video video = new Video()
        {
            Id = commentThreadEvent.VideoId,
            FirstComment = commentThreadEvent.FirstCommentId
        };
        return video;
    }
}