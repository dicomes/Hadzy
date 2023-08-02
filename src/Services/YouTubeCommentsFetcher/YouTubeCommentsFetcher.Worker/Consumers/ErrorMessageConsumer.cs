using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using MassTransit;

namespace YouTubeCommentsFetcher.Worker.Consumers;

public class ErrorMessageConsumer : IConsumer<IFetcherErrorEvent>
{
    private readonly ILogger<ErrorMessageConsumer> _logger;

    public ErrorMessageConsumer(ILogger<ErrorMessageConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IFetcherErrorEvent> context)
    {
        _logger.LogWarning("ErrorMessageConsumer: Handling an error occurred while fetching comments for VideoId: {VideoId}. ErrorMessage: {ErrorMessage}. ErrorCategory: {ErrorCategory}", context.Message.VideoId, context.Message.Message, context.Message.ErrorCategory);
    }
}