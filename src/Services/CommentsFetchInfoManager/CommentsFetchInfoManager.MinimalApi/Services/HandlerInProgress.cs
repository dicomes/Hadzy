using AutoMapper;
using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using IntegrationEventsContracts;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class HandlerInProgress : IFetchStatusHandler
{
    private readonly IMapper _mapper;

    public HandlerInProgress(IMapper mapper)
    {
        _mapper = mapper;
    }

    public bool CanHandle(string? status) => status == nameof(FetchStatus.InProgress);

    public Task<IResult> HandleAsync(FetchInfoDto fetchInfoDto, VideoFetchInfo oldVideoFetchInfo)
    {
        var fetchInfoDtoMapped = _mapper.Map<FetchInfoDto>(oldVideoFetchInfo);
        var response = new APIResponse<FetchInfoDto>
        {
            Result = fetchInfoDtoMapped
        };
        return Task.FromResult(Results.Accepted(value: response));
    }
}