using MassTransit;
using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services;

namespace YouTubeCommentsFetcher.Worker.Consumers;

public class VideoIdConsumer : IConsumer<IVideoIdMessage>
{
    private readonly ILogger<VideoIdConsumer> _logger;
    private readonly ICommentsService _commentsService;
    private readonly IPublishEndpoint _publishEndpoint;

    public VideoIdConsumer(ILogger<VideoIdConsumer> logger, ICommentsService commentsService, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _commentsService = commentsService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<IVideoIdMessage> context)
    {
        var videoId = context.Message.VideoId;
        string nextPageToken = null;

        do
        {
            var commentsFetchedEvent = await _commentsService.GetCommentsFetchedEventByVideoIdAsync(new FetchSettings(videoId, nextPageToken));
            _logger.LogInformation("Fetching batch completed for videoId: {VideoId}. PageToken: {PageToken}. Batch size: {BatchSize}", videoId, nextPageToken, commentsFetchedEvent.YouTubeCommentsList.Count);
            
            await _publishEndpoint.Publish<ICommentsFetchedEvent>(commentsFetchedEvent);
            _logger.LogInformation("Published fetched batch event completed for video: {VideoId}. PageToken: {PageToken}", videoId, nextPageToken);
            
            nextPageToken = commentsFetchedEvent.PageToken;
        } while (!string.IsNullOrEmpty(nextPageToken));
        
        _logger.LogInformation("Fetching all batchtes completed for videoId: {VideoId}.", videoId);

    }
}
