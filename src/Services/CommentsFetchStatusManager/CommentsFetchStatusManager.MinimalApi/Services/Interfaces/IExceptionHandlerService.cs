using CommentsFetchStatus.MinimalApi.Models;

namespace CommentsFetchStatus.MinimalApi.Services.Interfaces;

public interface IExceptionHandlerService
{
    IResult HandleException(Exception exception);
}