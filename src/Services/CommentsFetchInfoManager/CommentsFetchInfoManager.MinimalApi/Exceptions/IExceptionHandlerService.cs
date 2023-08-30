namespace CommentsFetchInfoManager.MinimalApi.Exceptions;

public interface IExceptionHandlerService
{
    IResult HandleException(Exception exception);
}