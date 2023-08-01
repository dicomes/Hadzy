using MassTransit;

namespace YouTubeCommentsFetcher.Worker.Services
{
    public interface ICommentsServiceExceptionHandler
    {
        public Task HandleError(Exception exception);
    }
}