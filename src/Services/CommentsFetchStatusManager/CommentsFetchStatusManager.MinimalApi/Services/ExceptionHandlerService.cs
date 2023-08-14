using System.Net;
using Amazon.Runtime;
using CommentsFetchStatus.MinimalApi.Models;
using CommentsFetchStatus.MinimalApi.Models.DTO;
using CommentsFetchStatus.MinimalApi.Services.Interfaces;

namespace CommentsFetchStatus.MinimalApi.Services;

public class ExceptionHandlerService : IExceptionHandlerService
{
    private readonly IFetchStatusHandlerService _fetchStatusHandlerService;

    public ExceptionHandlerService(IFetchStatusHandlerService fetchStatusHandlerService)
    {
        _fetchStatusHandlerService = fetchStatusHandlerService;
    }

    public IResult HandleException(Exception exception)
    {
        if (exception is BadHttpRequestException)
        {
            var errorResponse = _fetchStatusHandlerService.CreateErrorResponse<FetchInfoDto>("Message body is invalid");
            return Results.BadRequest(errorResponse);
        }
        return Results.StatusCode((int)HttpStatusCode.InternalServerError);
    }
}