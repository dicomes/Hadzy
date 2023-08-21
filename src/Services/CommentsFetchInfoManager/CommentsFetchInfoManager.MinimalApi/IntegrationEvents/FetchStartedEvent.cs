using IntegrationEventsContracts;

namespace CommentsFetchInfoManager.MinimalApi.IntegrationEvents;

public class FetchStartedEvent : IFetchStartedEvent
{
    public Guid Id { get; }
    public string? VideoId { get; }
    public string PageToken { get; set; }
    public List<string> FirstFetchedCommentIds { get; set; }

    public FetchStartedEvent(string? videoId)
    {
        Id = Guid.NewGuid();
        VideoId = videoId;
        PageToken = String.Empty;
        FirstFetchedCommentIds = new List<string>();
    }
}