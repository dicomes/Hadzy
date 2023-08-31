using AutoMapper;
using CommentsFetchInfoIntegration.Worker.Builders;
using CommentsFetchInfoIntegration.Worker.IntegrationEvents;
using CommentsFetchInfoIntegration.Worker.Models;
using CommentsFetchInfoIntegration.Worker.Repositories;
using CommentsFetchInfoIntegration.Worker.Services.Interfaces;
using IntegrationEventsContracts;

namespace CommentsFetchInfoIntegration.Worker.Services;

public class FetchInfoChangedEventHandler : IFetchInfoChangedEventHandler
{
    private readonly IFetchInfoService _fetchInfoService;
    private readonly IMapper _mapper;
    private readonly CommentsFetchInfoEventBuilder _eventBuilder;

    public FetchInfoChangedEventHandler(
        IFetchInfoService fetchInfoService,
        IMapper mapper,
        CommentsFetchInfoEventBuilder eventBuilder)
    {
        _fetchInfoService = fetchInfoService;
        _mapper = mapper;
        _eventBuilder = eventBuilder;
    }
    public async Task HandeAsync(IFetchInfoChangedEvent fetchInfoChangedEvent)
    {
        FetchInfoChangedEvent fetchInfoChanged = _eventBuilder.BuildFromEvent(fetchInfoChangedEvent);

        bool fetchInfoEventExists = await _fetchInfoService.ExistsByIdAsync(fetchInfoChanged.VideoId);

        if (fetchInfoEventExists)
        {
            await UpdateFetchInfoAsync(
                fetchInfoChanged.VideoId,
                fetchInfoChanged.Status,
                fetchInfoChanged.CommentsCount + fetchInfoChanged.ReplyCount,
                fetchInfoChanged.CommentIds,
                fetchInfoChanged.PageToken,
                fetchInfoChanged.CompletedTillFirstComment
                );
        }
        else
        {
            await InsertFetchInfoAsync(fetchInfoChanged);
        }
    }

    private async Task UpdateFetchInfoAsync(string? id, string? newStatus, ulong newTotalCommentsProcessed, List<string>? newCommentIds, string? newPageToken, bool completed)
    {
        var fetchInfo = await _fetchInfoService.GetByIdAsync(id);
        fetchInfo.Status = newStatus;
        fetchInfo.CommentsCount += newTotalCommentsProcessed;
        fetchInfo.CommentIds = newCommentIds ?? fetchInfo.CommentIds;
        fetchInfo.LastPageToken = newPageToken;
        fetchInfo.CompletedTillFirstComment = completed;
        await _fetchInfoService.UpdateAsync(fetchInfo);
    }

    private async Task InsertFetchInfoAsync(FetchInfoChangedEvent fetchInfoChangedEvent)
    {
        var fetchInfo = _mapper.Map<VideoFetchInfo>(fetchInfoChangedEvent);
        await _fetchInfoService.AddAsync(fetchInfo);
    }
}