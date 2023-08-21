using AutoMapper;
using CommentsFetchInfoIntegration.Worker.IntegrationEvents;
using CommentsFetchInfoIntegration.Worker.Models;
using CommentsFetchInfoIntegration.Worker.Services.Interfaces;

namespace CommentsFetchInfoIntegration.Worker.Services;

public class FetchInfoChangedEventHandler : IFetchInfoChangedEventHandler
{
    private readonly IFetchInfoService _fetchInfoService;
    private readonly IMapper _mapper;

    public FetchInfoChangedEventHandler(
        IFetchInfoService fetchInfoService,
        IMapper mapper)
    {
        _fetchInfoService = fetchInfoService;
        _mapper = mapper;
        
    }
    public async Task HandeAsync(FetchInfoChangedEvent fetchInfoChangedEvent)
    {

        bool fetchInfoEventExists = await _fetchInfoService.FetchInfoByIdExistsAsync(fetchInfoChangedEvent.VideoId);

        if (fetchInfoEventExists)
        {
            await UpdateFetchInfoAsync(
                fetchInfoChangedEvent.VideoId,
                fetchInfoChangedEvent.Status,
                fetchInfoChangedEvent.CommentsCount + fetchInfoChangedEvent.ReplyCount,
                fetchInfoChangedEvent.CommentIds,
                fetchInfoChangedEvent.PageToken,
                fetchInfoChangedEvent.CompletedTillFirstComment
                );
        }
        else
        {
            await InsertFetchInfoAsync(fetchInfoChangedEvent);
        }
    }

    private async Task UpdateFetchInfoAsync(string? id, string? newStatus, int newTotalCommentsProcessed, List<string>? newCommentIds, string? newPageToken, bool completed)
    {
        var fetchInfo = await _fetchInfoService.GetFetchInfoByIdAsync(id);
        fetchInfo.Status = newStatus;
        fetchInfo.CommentsCount += newTotalCommentsProcessed;
        fetchInfo.CommentIds = newCommentIds ?? fetchInfo.CommentIds;
        fetchInfo.LastPageToken = newPageToken;
        fetchInfo.CompletedTillFirstComment = completed;
        await _fetchInfoService.UpdateFetchInfoAsync(fetchInfo);
    }

    private async Task InsertFetchInfoAsync(FetchInfoChangedEvent fetchInfoChangedEvent)
    {
        var fetchInfo = _mapper.Map<VideoFetchInfo>(fetchInfoChangedEvent);
        await _fetchInfoService.InsertFetchInfoAsync(fetchInfo);
    }
}