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

    public async Task AddVideoByStartedEvent(IFetchStartedEvent fetchStarted)
    {
        var video = new Video()
        {
            Id = fetchStarted.VideoId
        };
        await _videoService.AddAsync(video);
    }

    public async Task UpdateVideoByCompletedEvent(IFetchCompletedEvent fetchCompleted)
    {
        if (string.IsNullOrEmpty(fetchCompleted.FirstCommentId))
        {
            return;
        }
        var video = new Video()
        {
            Id = fetchCompleted.VideoId,
            FirstComment = fetchCompleted.FirstCommentId
        };
        await _videoService.UpdateAsync(video);
    }
}