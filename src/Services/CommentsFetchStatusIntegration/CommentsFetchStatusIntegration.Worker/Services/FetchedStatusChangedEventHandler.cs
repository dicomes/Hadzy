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
    public async Task HandeAsync(FetchStatusChangedEvent fetchStatusChangedEvent)
    {

        bool fetchStatusByIdExistsAsync = await _fetchStatusService.FetchStatusByIdExistsAsync(fetchStatusChangedEvent.VideoId);

        if (fetchStatusByIdExistsAsync)
        {
            await UpdateVideoFetchStatusByIdAsync(
                fetchStatusChangedEvent.VideoId,
                fetchStatusChangedEvent.IsFetching,
                fetchStatusChangedEvent.CommentsFetchedCount + fetchStatusChangedEvent.ReplyCount,
                fetchStatusChangedEvent.PageToken);
        }
        else
        {
            await InsertVideoFetchStatusByStatusChangedEventAsync(fetchStatusChangedEvent);
        }
    }

    private async Task UpdateVideoFetchStatusByIdAsync(string id, bool newIsFetching, int newTotalCommentsProcessed, string newPageToken)
    {
        var videoFetchStatus = await _fetchStatusService.GetFetchStatusByIdAsync(id);
        videoFetchStatus.IsFetching = newIsFetching;
        videoFetchStatus.TotalCommentsProcessed += newTotalCommentsProcessed;
        videoFetchStatus.LastPageToken = newPageToken;
        await _fetchStatusService.UpdateFetchStatusAsync(videoFetchStatus);
    }

    private async Task InsertVideoFetchStatusByStatusChangedEventAsync(FetchStatusChangedEvent fetchStatusChangedEvent)
    {
        var videoFetchStatus = _mapper.Map<VideoFetchStatus>(fetchStatusChangedEvent);
        await _fetchStatusService.InsertFetchStatusAsync(videoFetchStatus);
    }
}