using AutoMapper;
using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;

namespace CommentsFetchStatusIntegration.Worker.Services;

public class FetchedStatusChangedEventHandler : IFetchedStatusChangedEventHandler
{
    private readonly IFetchStatusService _fetchStatusService;
    private readonly IMapper _mapper;

    public FetchedStatusChangedEventHandler(
        IFetchStatusService fetchStatusService,
        IMapper mapper)
    {
        _fetchStatusService = fetchStatusService;
        _mapper = mapper;
        
    }
    public async Task HandeAsync(FetchInfoChangedEvent fetchInfoChangedEvent)
    {

        bool fetchStatusByIdExistsAsync = await _fetchStatusService.FetchStatusByIdExistsAsync(fetchInfoChangedEvent.VideoId);

        if (fetchStatusByIdExistsAsync)
        {
            await UpdateVideoFetchStatusByIdAsync(
                fetchInfoChangedEvent.VideoId,
                fetchInfoChangedEvent.Status,
                fetchInfoChangedEvent.CommentsCount + fetchInfoChangedEvent.ReplyCount,
                fetchInfoChangedEvent.PageToken);
        }
        else
        {
            await InsertVideoFetchStatusByStatusChangedEventAsync(fetchInfoChangedEvent);
        }
    }

    private async Task UpdateVideoFetchStatusByIdAsync(string id, string newStatus, int newTotalCommentsProcessed, string newPageToken)
    {
        var videoFetchStatus = await _fetchStatusService.GetFetchStatusByIdAsync(id);
        videoFetchStatus.Status = newStatus;
        videoFetchStatus.CommentsCount += newTotalCommentsProcessed;
        videoFetchStatus.LastPageToken = newPageToken;
        await _fetchStatusService.UpdateFetchStatusAsync(videoFetchStatus);
    }

    private async Task InsertVideoFetchStatusByStatusChangedEventAsync(FetchInfoChangedEvent fetchInfoChangedEvent)
    {
        var videoFetchStatus = _mapper.Map<VideoFetchInfo>(fetchInfoChangedEvent);
        await _fetchStatusService.InsertFetchStatusAsync(videoFetchStatus);
    }
}