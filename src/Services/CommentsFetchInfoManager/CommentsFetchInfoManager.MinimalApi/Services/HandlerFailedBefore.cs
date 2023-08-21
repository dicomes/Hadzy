using CommentsFetchInfoManager.MinimalApi.IntegrationEvents;
using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using IntegrationEventsContracts;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class HandlerFailedBefore : IFetchStatusHandler
{
    private readonly IEventPublisherService _eventPublisherService;

    public HandlerFailedBefore(IEventPublisherService eventPublisherService)
    {
        _eventPublisherService = eventPublisherService;
    }

    public bool CanHandle(string? status) => status == nameof(FetchStatus.Failed);

    public async Task<IResult> HandleAsync(FetchInfoDto fetchInfoDto, VideoFetchInfo oldVideoFetchInfo)
    {
        var newFetchStartedEvent = new FetchStartedEvent(fetchInfoDto.VideoId)
        {
            PageToken = oldVideoFetchInfo.LastPageToken,
            FirstFetchedCommentIds = oldVideoFetchInfo.CommentIds
        };
        
        await _eventPublisherService.PublishEvent<IFetchStartedEvent>(newFetchStartedEvent);
        
        var response = new APIResponse<FetchInfoDto>
        {
            Result = new FetchInfoDto
            {
                VideoId = fetchInfoDto.VideoId,
                Status = FetchStatus.InProgress.ToString()
            }
        };
        return Results.Accepted(value: response);
    }
}
