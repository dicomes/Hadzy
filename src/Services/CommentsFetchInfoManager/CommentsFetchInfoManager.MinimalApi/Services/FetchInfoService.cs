using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using CommentsFetchInfoManager.MinimalApi.Validations;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class FetchInfoService : IFetchInfoService
{
    private readonly IValidationService<FetchInfoDto> _validationService;
    private readonly IFetchInfoRepository _fetchInfoRepository;
    private readonly IEnumerable<IFetchStatusHandler> _statusHandlers;
    private readonly IErrorResponseService _errorResponseService;

    public FetchInfoService(
        IValidationService<FetchInfoDto> validationService,
        IFetchInfoRepository fetchInfoRepository,
        IEnumerable<IFetchStatusHandler> statusHandlers,
        IErrorResponseService errorResponseService)
    {
        _validationService = validationService;
        _fetchInfoRepository = fetchInfoRepository;
        _statusHandlers = statusHandlers;
        _errorResponseService = errorResponseService;
    }

    public async Task<IResult> PostNewFetchInfo(FetchInfoDto? fetchInfoDto)
    {
        var apiResponse = new APIResponse<FetchInfoDto>();

        if (fetchInfoDto == null)
        {
            apiResponse = _errorResponseService.CreateErrorResponse<FetchInfoDto>(new List<string>{"Message body is missing"});
            return Results.BadRequest(apiResponse);
        }
        
        var validationResult = await _validationService.ValidateAsync(fetchInfoDto);
        
        if (!validationResult.IsValid)
        {
            apiResponse = _errorResponseService.CreateErrorResponse<FetchInfoDto>
                (validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            return Results.BadRequest(apiResponse);
        }

        VideoFetchInfo oldVideoFetchInfo = await _fetchInfoRepository.GetByVideoId(fetchInfoDto.VideoId);

        foreach (var handler in _statusHandlers)
        {
            if (handler.CanHandle(oldVideoFetchInfo?.Status))
            {
                return await handler.HandleAsync(fetchInfoDto, oldVideoFetchInfo);
            }
        }
        
        apiResponse = _errorResponseService.CreateErrorResponse<FetchInfoDto>(new List<string>{"Unknown status"});
        return Results.BadRequest(apiResponse);
    }
}

