using System.Net;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using FluentValidation;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class ExceptionHandlerService : IExceptionHandlerService
{
    private readonly IErrorResponseService _errorResponseService;

    public ExceptionHandlerService(
        IErrorResponseService errorResponseService)
    {
        _errorResponseService = errorResponseService;
    }

public IResult HandleException(Exception exception)
{
    if (exception is BadHttpRequestException)
    {
        var apiResponse = _errorResponseService.CreateError<FetchInfoDto>("Message body is invalid");
        return Results.BadRequest(apiResponse);
    }
    
    return Results.StatusCode((int)HttpStatusCode.InternalServerError);
}

}