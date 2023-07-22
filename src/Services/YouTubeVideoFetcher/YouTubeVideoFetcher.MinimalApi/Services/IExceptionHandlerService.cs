namespace YouTubeVideoFetcher.MinimalApi.Services;

public interface IExceptionHandlerService
{
    Task<IResult> HandleException(Exception exception);
}