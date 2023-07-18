namespace YouTubeVideoFetcher.Services;

public interface IExceptionHandlerService
{
    Task<IResult> HandleException(Exception exception);
}