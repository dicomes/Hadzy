namespace CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

public interface IExceptionHandlerService
{
    IResult HandleException(Exception exception);
}