using AutoMapper;
using CommentsFetchInfoManager.MinimalApi.IntegrationEvents;
using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using IntegrationEventsContracts;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class FetchInfoHandlerService : IFetchInfoHandlerService
{
    private readonly IFetchInfoRepository _fetchInfoRepository;
    private readonly IFetchInfoService _fetchInfoService;
    private readonly IMapper _mapper;

    public FetchInfoHandlerService(
        IFetchInfoService fetchInfoService,
        IMapper mapper,
        IFetchInfoRepository fetchInfoRepository
        )
    {
        _fetchInfoRepository = fetchInfoRepository;
        _fetchInfoService = fetchInfoService;
        _mapper = mapper;
    }

    public async Task<IResult> GetFetchStatusByIdAsync(string videoId)
    {
        if (string.IsNullOrEmpty(videoId))
        {
            var apiResponse = CreateErrorResponse<FetchInfoDto>("Video Id cannot be blank");
            return Results.BadRequest(apiResponse);
        }

        // You might need to refactor this part if FetchInfoService has a method for this.
        VideoFetchInfo videoFetchInfo = await _fetchInfoRepository.GetByVideoId(videoId);
        
        if (videoFetchInfo == null)
        {
            var apiResponse = CreateErrorResponse<FetchInfoDto>("Video Id not found");
            return Results.NotFound(apiResponse);
        }

        var fetchInfoDto = _mapper.Map<FetchInfoDto>(videoFetchInfo);
        var response = new APIResponse<FetchInfoDto>
        {
            Result = fetchInfoDto
        };

        return Results.Ok(response);
    }

    public async Task<IResult> PostNewFetchInfo(FetchInfoDto fetchInfoDto)
    {
        return await _fetchInfoService.PostNewFetchInfo(fetchInfoDto);
    }
    
    public APIResponse<T> CreateErrorResponse<T>(string errorMessage)
    {
        var response = new APIResponse<T>()
        {
            Result = default
        };

        if (!string.IsNullOrEmpty(errorMessage))
        {
            response.ErrorMessages.Add(errorMessage);
        }

        return response;
    }
}
