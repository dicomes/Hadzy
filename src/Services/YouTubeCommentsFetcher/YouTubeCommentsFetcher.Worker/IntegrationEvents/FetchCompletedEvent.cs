using IntegrationEventsContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class FetchCompletedEvent : IFetchCompletedEvent
{
    public FetchCompletedEvent()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public string VideoId { get; set; }
    public string? FirstCommentId { get; set; }
}