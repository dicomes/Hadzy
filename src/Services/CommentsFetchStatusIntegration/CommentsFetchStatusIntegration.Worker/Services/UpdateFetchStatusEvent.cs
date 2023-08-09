using AutoMapper;
using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;

namespace CommentsFetchStatusIntegration.Worker.Services;

public class UpdateFetchStatusEvent : IUpdateFetchStatusEvent
{
    private readonly IFetchStatusService _fetchStatusService;
    private readonly IMapper _mapper;

    public UpdateFetchStatusEvent(IFetchStatusService fetchStatusService, IMapper mapper)
    {
        _fetchStatusService = fetchStatusService;
        _mapper = mapper;
        
    }
    public async Task UpdateAsync(FetchStatusChangedEvent fetchStatusChangedEvent)
    {
        FetchStatus fetchStatus;

        bool videoIdExists = await _fetchStatusService.VideoIdExistsAsync(fetchStatusChangedEvent.VideoId);

        if (videoIdExists)
        {
            fetchStatus = await _fetchStatusService.GetFetchStatusByVideoIdAsync(fetchStatusChangedEvent.VideoId);
            fetchStatus.IsFetching = fetchStatusChangedEvent.IsFetching;
            fetchStatus.TotalCommentsFetched += fetchStatusChangedEvent.CommentsFetchedCount + fetchStatusChangedEvent.ReplyCount;
            fetchStatus.LastPageToken = fetchStatusChangedEvent.PageToken;
            await _fetchStatusService.UpdateFetchStatusAsync(fetchStatus);
        }
        else
        {
            fetchStatus = _mapper.Map<FetchStatus>(fetchStatusChangedEvent);
            await _fetchStatusService.InsertFetchStatusAsync(fetchStatus);
        }
    }
}