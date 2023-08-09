using MassTransit;
using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Consumers
{
    public class FetchingInitiatedEventConsumer : IConsumer<IFetchingInitiatedEvent>
    {
        private readonly ILogger<FetchingInitiatedEventConsumer> _logger;
        private readonly IFetchEventsIntegration _fetchEventsIntegration;
        private readonly IYouTubeFetcherServiceExceptionHandler _youTubeFetcherServiceExceptionHandler;

        public FetchingInitiatedEventConsumer(
            ILogger<FetchingInitiatedEventConsumer> logger, 
            IFetchEventsIntegration fetchEventsIntegration, 
            IYouTubeFetcherServiceExceptionHandler youTubeFetcherServiceExceptionHandler)
        {
            _logger = logger;
            _fetchEventsIntegration = fetchEventsIntegration;
            _youTubeFetcherServiceExceptionHandler = youTubeFetcherServiceExceptionHandler;
        }

        public async Task Consume(ConsumeContext<IFetchingInitiatedEvent> context)
        {
            var videoId = context.Message.VideoId;
            string nextPageToken = context.Message.PageToken;
            _logger.LogInformation("FetchingInitiatedEventConsumer: Received FetchingInitiatedEvent: {VideoId}. PageToken: {PageToken}.", videoId, nextPageToken);
        
            try
            {
                await _fetchEventsIntegration.FetchAndPublishAsync(videoId, nextPageToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "FetchingInitiatedEventConsumer: Raised a {ExceptionType} while processing videoId: {VideoId}. Exception Message: {ExceptionMessage}.", ex.GetType().Name, videoId, ex.Message);
                await _youTubeFetcherServiceExceptionHandler.HandleError(ex);
            }
        }
    }
}