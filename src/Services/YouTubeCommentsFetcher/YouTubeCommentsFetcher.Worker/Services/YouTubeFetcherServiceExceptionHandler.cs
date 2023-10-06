using YouTubeCommentsFetcher.Worker.Contracts;
using YouTubeCommentsFetcher.Worker.Exceptions;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;

namespace YouTubeCommentsFetcher.Worker.Services
{
    public class YouTubeFetcherServiceExceptionHandler : IYouTubeFetcherServiceExceptionHandler
    {
        private readonly IEventPublisher _eventPublisher;

        public YouTubeFetcherServiceExceptionHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task HandleError(Exception exception)
        {
            if (exception is YouTubeFetcherServiceException commentsServiceException)
            {
                var fetcherErrorEvent = new FetcherErrorEvent
                {
                    Message = commentsServiceException.Message,
                    ErrorCategory = commentsServiceException.ErrorCategory,
                    VideoId = commentsServiceException.VideoId,
                };

                await _eventPublisher.PublishEvent(fetcherErrorEvent);
            }
        }
    }
}