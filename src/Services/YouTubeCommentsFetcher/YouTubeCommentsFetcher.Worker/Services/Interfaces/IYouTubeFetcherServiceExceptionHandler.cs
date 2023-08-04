namespace YouTubeCommentsFetcher.Worker.Services.Interfaces
{
    public interface IYouTubeFetcherServiceExceptionHandler
    {
        public Task HandleError(Exception exception);
    }
}