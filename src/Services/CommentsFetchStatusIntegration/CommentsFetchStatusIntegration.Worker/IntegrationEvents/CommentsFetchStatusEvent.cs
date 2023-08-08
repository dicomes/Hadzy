using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.IntegrationEvents;

public class CommentsFetchStatusEvent : ICommentsFetchStatusEvent
{
    public Guid Id { get; set; }
    public string VideoId { get; set; }
    public string PageToken { get; set; }
    public int CommentsFetchedCount { get; set; }
    public int ReplyCount { get; set; }
    public bool IsFetching { get; set; }
}