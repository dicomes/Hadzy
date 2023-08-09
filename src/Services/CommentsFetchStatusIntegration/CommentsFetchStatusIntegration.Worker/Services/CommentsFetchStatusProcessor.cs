using AutoMapper;
using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;

namespace CommentsFetchStatusIntegration.Worker.Services;

public class CommentsFetchStatusProcessor : ICommentsFetchStatusProcessor
{
    private readonly IVideoCommentsStatusService _videoCommentsStatusService;
    private readonly IMapper _mapper;

    public CommentsFetchStatusProcessor(IVideoCommentsStatusService videoCommentsStatusService, IMapper mapper)
    {
        _videoCommentsStatusService = videoCommentsStatusService;
        _mapper = mapper;
        
    }
    public async Task ProcessVideoCommentsStatusAsync(CommentsFetchStatusEvent fetchStatusEvent)
    {
        VideoCommentsStatus videoCommentsStatus;

        bool videoIdExists = await _videoCommentsStatusService.VideoIdExistsAsync(fetchStatusEvent.VideoId);

        if (videoIdExists)
        {
            videoCommentsStatus = await _videoCommentsStatusService.GetVideoCommentsStatusByVideoIdAsync(fetchStatusEvent.VideoId);
            videoCommentsStatus.IsFetching = fetchStatusEvent.IsFetching;
            videoCommentsStatus.TotalCommentsFetched += fetchStatusEvent.CommentsFetchedCount + fetchStatusEvent.ReplyCount;
            await _videoCommentsStatusService.UpdateVideoCommentsStatusAsync(videoCommentsStatus);
        }
        else
        {
            videoCommentsStatus = _mapper.Map<VideoCommentsStatus>(fetchStatusEvent);
            await _videoCommentsStatusService.InsertVideoCommentsStatusAsync(videoCommentsStatus);
        }
    }
}