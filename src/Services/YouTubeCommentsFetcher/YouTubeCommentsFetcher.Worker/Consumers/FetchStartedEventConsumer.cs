using Google;
using MassTransit;
using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.Exceptions;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Consumers
{
    public class FetchStartedEventConsumer : IConsumer<IFetchStartedEvent>
    {
        private readonly ILogger<FetchStartedEventConsumer> _logger;
        private readonly IIntegrationEventsManager _integrationEventsManager;
        private readonly IYouTubeFetcherServiceExceptionHandler _youTubeFetcherServiceExceptionHandler;

        public FetchStartedEventConsumer(
            ILogger<FetchStartedEventConsumer> logger, 
            IIntegrationEventsManager integrationEventsManager, 
            IYouTubeFetcherServiceExceptionHandler youTubeFetcherServiceExceptionHandler)
        {
            _logger = logger;
            _integrationEventsManager = integrationEventsManager;
            _youTubeFetcherServiceExceptionHandler = youTubeFetcherServiceExceptionHandler;
        }

        public async Task Consume(ConsumeContext<IFetchStartedEvent> context)
        {
            var videoId = context.Message.VideoId;
            string nextPageToken = context.Message.PageToken;
            _logger.LogInformation("FetchStartedEventConsumer: Received FetchingInitiatedEvent: {VideoId}. PageToken: {PageToken}.", videoId, nextPageToken);
        
            try
            {
                await _integrationEventsManager.FetchCommentsAndPublishFetchedEventsAsync(videoId, nextPageToken);
            }
            catch (YouTubeFetcherServiceException ex)
            {
                _logger.LogWarning(ex, "FetchStartedEventConsumer: Raised a {ExceptionType} while processing videoId: {VideoId}. Exception Message: {ExceptionMessage}.", ex.GetType().Name, videoId, ex.Message);
                await _youTubeFetcherServiceExceptionHandler.HandleError(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FetchStartedEventConsumer: Raised a {ExceptionType} while processing videoId: {VideoId}. Exception Message: {ExceptionMessage}.", ex.GetType().Name, videoId, ex.Message);
            }
        }
    }
}