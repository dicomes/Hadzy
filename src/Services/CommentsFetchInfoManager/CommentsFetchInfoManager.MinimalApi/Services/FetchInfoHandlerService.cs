using AutoMapper;
using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class FetchInfoHandlerService : IFetchInfoHandlerService
{
    private readonly IFetchInfoRepository _fetchInfoRepository;
    private readonly IFetchInfoService _fetchInfoService;
    private readonly IMapper _mapper;
    private readonly IErrorResponseService _errorResponseService;

    public FetchInfoHandlerService(
        IFetchInfoService fetchInfoService,
        IMapper mapper,
        IFetchInfoRepository fetchInfoRepository,
        IErrorResponseService errorResponseService)
    {
        _fetchInfoRepository = fetchInfoRepository;
        _fetchInfoService = fetchInfoService;
        _mapper = mapper;
        _errorResponseService = errorResponseService;
    }

    public async Task<IResult> GetFetchStatusByIdAsync(string? videoId)
    {
        if (string.IsNullOrEmpty(videoId))
        {
            var apiResponse = _errorResponseService.CreateErrorResponse<FetchInfoDto>(new List<string>{"Video Id cannot be blank"});
            return Results.BadRequest(apiResponse);
        }

        VideoFetchInfo videoFetchInfo = await _fetchInfoRepository.GetByVideoId(videoId);
        
        if (videoFetchInfo == null)
        {
            var apiResponse = _errorResponseService.CreateErrorResponse<FetchInfoDto>(new List<string>{"Video Id not found"});
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
}
