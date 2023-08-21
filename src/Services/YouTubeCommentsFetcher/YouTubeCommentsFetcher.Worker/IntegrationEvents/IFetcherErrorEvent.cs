using YouTubeCommentsFetcher.Worker.Enums;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents
{
    public interface IFetcherErrorEvent
    {
        Guid Id { get; }
        string Message { get; }
        ErrorCategory ErrorCategory { get; }
        string? VideoId { get; }
    }
}