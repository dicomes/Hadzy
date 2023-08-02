using MassTransit;
using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services;

namespace YouTubeCommentsFetcher.Worker.Consumers;

public class VideoIdConsumer : IConsumer<IFetchCommentsEvent>
{
    private readonly ILogger<VideoIdConsumer> _logger;
    private readonly ICommentsService _commentsService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICommentsServiceExceptionHandler _commentsServiceExceptionHandler;

    public VideoIdConsumer(ILogger<VideoIdConsumer> logger, ICommentsService commentsService, IPublishEndpoint publishEndpoint, ICommentsServiceExceptionHandler commentsServiceExceptionHandler)
    {
        _logger = logger;
        _commentsService = commentsService;
        _publishEndpoint = publishEndpoint;
        _commentsServiceExceptionHandler = commentsServiceExceptionHandler;
    }

    public async Task Consume(ConsumeContext<IFetchCommentsEvent> context)
    {
        var videoId = context.Message.VideoId;
        string nextPageToken = null;
        _logger.LogInformation("VideoIdConsumer: VideoId received: {VideoId}. PageToken: {PageToken}.",videoId, nextPageToken);

        try
        {
            do
            {
                var commentsFetchedEvent = await _commentsService.GetCommentsFetchedEventByVideoIdAsync(new FetchSettings(videoId, nextPageToken));
                _logger.LogInformation("VideoIdConsumer: Fetching batch completed for videoId: {VideoId}. PageToken: {PageToken}. Batch size: {BatchSize}", videoId, nextPageToken, commentsFetchedEvent.YouTubeCommentsList.Count);
            
                await _publishEndpoint.Publish<ICommentsFetchedEvent>(commentsFetchedEvent);
                _logger.LogInformation("VideoIdConsumer: Published fetched batch event completed for video: {VideoId}. PageToken: {PageToken}", videoId, nextPageToken);
            
                nextPageToken = commentsFetchedEvent.PageToken;
            } while (!string.IsNullOrEmpty(nextPageToken));
        
            _logger.LogInformation("VideoIdConsumer: Fetching all batches completed for videoId: {VideoId}.", videoId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "VideoIdConsumer: Caught a {ExceptionType} while processing videoId: {VideoId}. Exception Message: {ExceptionMessage}", ex.GetType().Name, videoId, ex.Message);
            await _commentsServiceExceptionHandler.HandleError(ex);
        }

    }

}
