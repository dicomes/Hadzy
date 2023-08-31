using System.Net;
using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Repositories;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using CommentsFetchInfoManager.MinimalApi.Validations;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class FetchInfoHandlerService : IFetchInfoHandlerService
{
    private readonly IValidationService<FetchInfoDto> _validationService;
    private readonly IFetchInfoRepository _fetchInfoRepository;
    private readonly IEnumerable<IFetchStatusHandler> _statusHandlers;
    private readonly IErrorResponseService _errorResponseService;

    public FetchInfoHandlerService(
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

    public async Task<IResult> HandleAsync(FetchInfoDto? fetchInfoDto)
    {
        VideoFetchInfo oldVideoFetchInfo = await _fetchInfoRepository.GetByIdAsync(fetchInfoDto.VideoId);

        foreach (var handler in _statusHandlers)
        {
            if (handler.CanHandle(oldVideoFetchInfo?.Status))
            {
                return await handler.HandleAsync(fetchInfoDto, oldVideoFetchInfo);
            }
        }
        
        return Results.StatusCode((int)HttpStatusCode.InternalServerError);
    }

}

