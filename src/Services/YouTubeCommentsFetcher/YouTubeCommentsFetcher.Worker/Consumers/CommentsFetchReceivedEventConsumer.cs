using MassTransit;
using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Consumers
{
    public class CommentsFetchReceivedEventConsumer : IConsumer<ICommentsFetchReceivedEvent>
    {
        private readonly ILogger<CommentsFetchReceivedEventConsumer> _logger;
        private readonly ICommentsIntegrationOrchestrator _commentsIntegrationOrchestrator;
        private readonly IYouTubeFetcherServiceExceptionHandler _youTubeFetcherServiceExceptionHandler;

        public CommentsFetchReceivedEventConsumer(
            ILogger<CommentsFetchReceivedEventConsumer> logger, 
            ICommentsIntegrationOrchestrator commentsIntegrationOrchestrator, 
            IYouTubeFetcherServiceExceptionHandler youTubeFetcherServiceExceptionHandler)
        {
            _logger = logger;
            _commentsIntegrationOrchestrator = commentsIntegrationOrchestrator;
            _youTubeFetcherServiceExceptionHandler = youTubeFetcherServiceExceptionHandler;
        }

        public async Task Consume(ConsumeContext<ICommentsFetchReceivedEvent> context)
        {
            var videoId = context.Message.VideoId;
            string nextPageToken = context.Message.PageToken;
            _logger.LogInformation("CommentsFetchReceivedEventConsumer: Received VideoId: {VideoId}. PageToken: {PageToken}.", videoId, nextPageToken);
        
            try
            {
                await _commentsIntegrationOrchestrator.ProcessCommentsForVideoAsync(videoId, nextPageToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "CommentsFetchReceivedEventConsumer: Raised a {ExceptionType} while processing videoId: {VideoId}. Exception Message: {ExceptionMessage}.", ex.GetType().Name, videoId, ex.Message);
                await _youTubeFetcherServiceExceptionHandler.HandleError(ex);
            }
        }
    }
}