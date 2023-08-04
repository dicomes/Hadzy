using YouTubeCommentsFetcher.Worker.Exceptions;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services
{
    public class YouTubeFetcherServiceExceptionHandler : IYouTubeFetcherServiceExceptionHandler
    {
        private readonly IErrorEventPublisher _errorEventPublisher;

        public YouTubeFetcherServiceExceptionHandler(IErrorEventPublisher errorEventPublisher)
        {
            _errorEventPublisher = errorEventPublisher;
        }

        public async Task HandleError(Exception exception)
        {
            if (exception is YouTubeFetcherServiceException commentsServiceException)
            {
                var internalErrorEvent = new FetcherErrorEvent
                {
                    Message = commentsServiceException.Message,
                    ErrorCategory = commentsServiceException.ErrorCategory,
                    VideoId = commentsServiceException.VideoId,
                };

                await _errorEventPublisher.PublishErrorEvent(internalErrorEvent);
            }
        }
    }
}