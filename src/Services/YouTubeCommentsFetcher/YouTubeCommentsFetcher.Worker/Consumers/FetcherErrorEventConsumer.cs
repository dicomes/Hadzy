using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using MassTransit;

namespace YouTubeCommentsFetcher.Worker.Consumers;

public class FetcherErrorEventConsumer : IConsumer<IInternalFetcherErrorEvent>
{
    private readonly ILogger<FetcherErrorEventConsumer> _logger;

    public FetcherErrorEventConsumer(ILogger<FetcherErrorEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IInternalFetcherErrorEvent> context)
    {
        _logger.LogWarning("FetcherErrorEventConsumer: Handling an error occurred while fetching comments for VideoId: {VideoId}. ErrorMessage: {ErrorMessage}. ErrorCategory: {ErrorCategory}", context.Message.VideoId, context.Message.Message, context.Message.ErrorCategory);
    }
}