using YouTubeCommentsFetcher.Worker.Enums;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class FetcherErrorEvent : IFetcherErrorEvent
{
    public Guid Id { get; }
    public string Message { get; set; }
    public ErrorCategory ErrorCategory { get; set; }
    public string VideoId { get; set; }
}
