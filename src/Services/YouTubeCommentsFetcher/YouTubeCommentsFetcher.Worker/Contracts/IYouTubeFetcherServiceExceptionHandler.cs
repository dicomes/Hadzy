namespace YouTubeCommentsFetcher.Worker.Contracts;

public interface IYouTubeFetcherServiceExceptionHandler
{
    public Task HandleError(Exception exception);
}
