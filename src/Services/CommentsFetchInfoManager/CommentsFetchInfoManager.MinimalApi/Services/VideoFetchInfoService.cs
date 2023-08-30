using AutoMapper;
using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Repositories;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using CommentsFetchInfoManager.MinimalApi.Validations;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class VideoFetchInfoService : IVideoFetchInfoService
{
    private readonly IFetchInfoHandlerService _fetchInfoHandlerService;
    private readonly IMapper _mapper;
    private readonly IVideoFetchInfoRepository _videoFetchInfoRepository;
    private readonly IErrorResponseService _errorResponseService;
    private readonly IValidationService<FetchInfoDto> _validationService;

    public VideoFetchInfoService(
        IFetchInfoHandlerService fetchInfoHandlerService,
        IMapper mapper,
        IVideoFetchInfoRepository videoFetchInfoRepository,
        IErrorResponseService errorResponseService,
        IValidationService<FetchInfoDto> validationService)
    {
        _fetchInfoHandlerService = fetchInfoHandlerService;
        _mapper = mapper;
        _videoFetchInfoRepository = videoFetchInfoRepository;
        _errorResponseService = errorResponseService;
        _validationService = validationService;
    }

    public async Task<IResult> GetFetchInfoByIdAsync(string videoId)
    {
        VideoFetchInfo videoFetchInfo = await _videoFetchInfoRepository.GetByIdAsync(videoId);
    
        if (videoFetchInfo == null)
        {
            var apiResponse = _errorResponseService.CreateError<FetchInfoDto>("Video Id not found");
            return Results.NotFound(apiResponse);
        }

        var fetchInfoDto = _mapper.Map<FetchInfoDto>(videoFetchInfo);
        var response = new APIResponse<FetchInfoDto>
        {
            Result = fetchInfoDto
        };

        return Results.Ok(response);
    }


    public async Task<IResult> CreateNewFetchInfoAsync(FetchInfoDto fetchInfoDto)
    {
        var validationResult = await _validationService.ValidateAsync(fetchInfoDto);
        if (validationResult.IsValid) return await _fetchInfoHandlerService.HandleAsync(fetchInfoDto);
        
        var apiResponse = new APIResponse<FetchInfoDto>();
        apiResponse = _errorResponseService.CreateError<FetchInfoDto>
            (validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        return Results.BadRequest(apiResponse);
    }
}
