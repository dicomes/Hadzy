using System.Net;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class ExceptionHandlerService : IExceptionHandlerService
{
    private readonly IFetchInfoHandlerService _fetchInfoHandlerService;
    private readonly IErrorResponseService _errorResponseService;

    public ExceptionHandlerService(
        IFetchInfoHandlerService fetchInfoHandlerService,
        IErrorResponseService errorResponseService)
    {
        _fetchInfoHandlerService = fetchInfoHandlerService;
        _errorResponseService = errorResponseService;
    }

    public IResult HandleException(Exception exception)
    {
        if (exception is BadHttpRequestException)
        {
            var apiResponse = _errorResponseService.CreateErrorResponse<FetchInfoDto>(new List<string>{"Message body is invalid"});
            return Results.BadRequest(apiResponse);
        }
        
        return Results.StatusCode((int)HttpStatusCode.InternalServerError);
    }
}