using MassTransit;
using YouTubeCommentsFetcher.Worker.Exceptions;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services
{
    public class YouTubeFetcherServiceExceptionHandler : IYouTubeFetcherServiceExceptionHandler
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public YouTubeFetcherServiceExceptionHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task HandleError(Exception exception)
        {
            if (exception is YouTubeFetcherServiceException commentsServiceException)
            {
                await _publishEndpoint.Publish<IInternalFetcherErrorEvent>(new
                {
                    Message = commentsServiceException.Message,
                    ErrorCategory = commentsServiceException.ErrorCategory,
                    VideoId = commentsServiceException.VideoId,
                });
            }
        }
    }
}