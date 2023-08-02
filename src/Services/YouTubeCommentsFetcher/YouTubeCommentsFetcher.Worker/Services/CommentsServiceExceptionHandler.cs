using MassTransit;
using YouTubeCommentsFetcher.Worker.Exceptions;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;

namespace YouTubeCommentsFetcher.Worker.Services
{
    public class CommentsServiceExceptionHandler : ICommentsServiceExceptionHandler
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public CommentsServiceExceptionHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task HandleError(Exception exception)
        {
            if (exception is CommentsServiceException commentsServiceException)
            {
                await _publishEndpoint.Publish<IFetcherErrorEvent>(new
                {
                    Message = commentsServiceException.Message,
                    ErrorType = commentsServiceException.ErrorCategory,
                    VideoId = commentsServiceException.VideoId,
                });
            }
        }
    }
}