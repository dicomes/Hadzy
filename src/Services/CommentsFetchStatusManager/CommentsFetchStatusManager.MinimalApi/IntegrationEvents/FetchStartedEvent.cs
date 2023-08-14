using IntegrationEventsContracts;

namespace CommentsFetchStatus.MinimalApi.IntegrationEvents;

public class FetchStartedEvent : IFetchStartedEvent
{
    public Guid Id { get; }
    public string VideoId { get; set; }
    public string PageToken { get; set; }

    public FetchStartedEvent(string videoId)
    {
        Id = new Guid();
        VideoId = videoId;
    }
}