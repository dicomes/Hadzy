namespace CommentsFetchStatus.MinimalApi.Services.Interfaces;

public interface IExceptionHandlerService
{
    Task<IResult> HandleException(Exception exception);
}