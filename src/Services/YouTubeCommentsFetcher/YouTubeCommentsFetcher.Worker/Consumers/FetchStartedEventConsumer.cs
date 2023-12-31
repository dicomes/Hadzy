using Google;
using MassTransit;
using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.Contracts;
using YouTubeCommentsFetcher.Worker.Exceptions;

namespace YouTubeCommentsFetcher.Worker.Consumers
{
    public class FetchStartedEventConsumer : IConsumer<IFetchStartedEvent>
    {
        private readonly ILogger<FetchStartedEventConsumer> _logger;
        private readonly IEventsManager _eventsManager;
        private readonly IYouTubeFetcherServiceExceptionHandler _youTubeFetcherServiceExceptionHandler;

        public FetchStartedEventConsumer(
            ILogger<FetchStartedEventConsumer> logger, 
            IEventsManager eventsManager, 
            IYouTubeFetcherServiceExceptionHandler youTubeFetcherServiceExceptionHandler)
        {
            _logger = logger;
            _eventsManager = eventsManager;
            _youTubeFetcherServiceExceptionHandler = youTubeFetcherServiceExceptionHandler;
        }

        public async Task Consume(ConsumeContext<IFetchStartedEvent> context)
        {
            
            _logger.LogInformation("{Source}: Received FetchingInitiatedEvent: {VideoId}. NextPageToken: {PageToken}.", GetType().Name, context.Message.VideoId, context.Message.PageToken);
        
            try
            {
                await _eventsManager.IterateCommentsAndPublishEventsAsync(context.Message.VideoId, context.Message.PageToken, context.Message.FirstFetchedCommentIds);
            }
            catch (YouTubeFetcherServiceException ex)
            {
                _logger.LogWarning(ex, "{Source}: Raised a {ExceptionType} while processing videoId: {VideoId}. Exception Message: {ExceptionMessage}.", GetType().Name, ex.GetType().Name, context.Message.VideoId, ex.Message);
                await _youTubeFetcherServiceExceptionHandler.HandleError(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Source}: Raised a {ExceptionType} while processing videoId: {VideoId}. Exception Message: {ExceptionMessage}.", GetType().Name, ex.GetType().Name, context.Message.VideoId, ex.Message);
            }
        }
    }
}