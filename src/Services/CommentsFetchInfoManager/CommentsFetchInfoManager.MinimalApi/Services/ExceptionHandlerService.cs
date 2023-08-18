using System.Net;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class ExceptionHandlerService : IExceptionHandlerService
{
    private readonly IFetchInfoHandlerService _fetchInfoHandlerService;

    public ExceptionHandlerService(IFetchInfoHandlerService fetchInfoHandlerService)
    {
        _fetchInfoHandlerService = fetchInfoHandlerService;
    }

    public IResult HandleException(Exception exception)
    {
        if (exception is BadHttpRequestException)
        {
            var errorResponse = _fetchInfoHandlerService.CreateErrorResponse<FetchInfoDto>("Message body is invalid");
            return Results.BadRequest(errorResponse);
        }
        return Results.StatusCode((int)HttpStatusCode.InternalServerError);
    }
}