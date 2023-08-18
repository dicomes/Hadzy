using AutoMapper;
using CommentsFetchInfoManager.MinimalApi.IntegrationEvents;
using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using CommentsFetchInfoManager.MinimalApi.Validations;
using IntegrationEventsContracts;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class FetchInfoService : IFetchInfoService
{
    private readonly IValidationService<FetchInfoDto> _validationService;
    private readonly IFetchInfoRepository _fetchInfoRepository;
    private readonly IEventPublisherService _eventPublisherService;
    private readonly IMapper _mapper;

    public FetchInfoService(
        IValidationService<FetchInfoDto> validationService,
        IFetchInfoRepository fetchInfoRepository,
        IEventPublisherService eventPublisherService,
        IMapper mapper)
    {
        _validationService = validationService;
        _fetchInfoRepository = fetchInfoRepository;
        _eventPublisherService = eventPublisherService;
        _mapper = mapper;
    }

    public async Task<IResult> PostNewFetchInfo(FetchInfoDto? fetchInfoDto)
    {
        var response = new APIResponse<FetchInfoDto>();

        if (fetchInfoDto == null)
        {
            response.ErrorMessages.Add("Request body is missing");
            return Results.BadRequest(response);
        }
        
        var validationResult = await _validationService.ValidateAsync(fetchInfoDto);
        
        if (!validationResult.IsValid)
        {
            response.ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Results.BadRequest(response);
        }

        VideoFetchInfo oldVideoFetchInfo = await _fetchInfoRepository.GetByVideoId(fetchInfoDto.VideoId);

        switch (oldVideoFetchInfo?.Status)
        {
            case null:
                return HandleNewFetch(fetchInfoDto, response);
            case nameof(FetchStatus.InProgress):
                return HandleInProgressFetch(oldVideoFetchInfo, response);
            case nameof(FetchStatus.Done):
                return HandleCompletedFetch(fetchInfoDto, oldVideoFetchInfo, response);
            default:
                return Results.Accepted(value: response);
        }
    }

    private IResult HandleNewFetch(FetchInfoDto fetchInfoDto, APIResponse<FetchInfoDto> response)
    {
        var newFetchStartedEvent = new FetchStartedEvent(fetchInfoDto.VideoId);
        _eventPublisherService.PublishEvent<IFetchStartedEvent>(newFetchStartedEvent);
        response.Result = new FetchInfoDto()
        {
            VideoId = fetchInfoDto.VideoId,
            Status = FetchStatus.InProgress.ToString()
        };
        return Results.Accepted(value: response);
    }

    private IResult HandleInProgressFetch(VideoFetchInfo oldVideoFetchInfo, APIResponse<FetchInfoDto> response)
    {
        var newFetchInfoDto = _mapper.Map<FetchInfoDto>(oldVideoFetchInfo);
        response.Result = newFetchInfoDto;
        return Results.Accepted(value: response);
    }

    private IResult HandleCompletedFetch(FetchInfoDto fetchInfoDto, VideoFetchInfo oldVideoFetchInfo, APIResponse<FetchInfoDto> response)
    {
        var newFetchStartedEvent = new FetchStartedEvent(fetchInfoDto.VideoId)
        {
            CommentIds = oldVideoFetchInfo.CommentIds
        };
        _eventPublisherService.PublishEvent<IFetchStartedEvent>(newFetchStartedEvent);
        response.Result = new FetchInfoDto()
        {
            VideoId = fetchInfoDto.VideoId,
            Status = FetchStatus.InProgress.ToString()
        };
        return Results.Accepted(value: response);
    }
}
