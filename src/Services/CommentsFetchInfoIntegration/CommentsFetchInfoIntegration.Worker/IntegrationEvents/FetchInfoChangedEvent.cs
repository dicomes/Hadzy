using IntegrationEventsContracts;

namespace CommentsFetchInfoIntegration.Worker.IntegrationEvents;

public class FetchInfoChangedEvent : IFetchInfoChangedEvent
{
    public Guid Id { get; set; }
    public string? VideoId { get; set; }
    public string? PageToken { get; set; }
    public ulong CommentsCount { get; set; }
    public List<string>? CommentIds { get; set; }
    public uint ReplyCount { get; set; }
    public string? Status { get; set; }
    public bool CompletedTillFirstComment { get; set;  }

    public override string ToString() =>
        $"FetchInfoChangedEvent - Guid: {Id}. VideoId: {VideoId}, NextPageToken: {PageToken}, " +
        $"CommentsCount: {CommentsCount}, ReplyCount: {ReplyCount}, Status: {Status}";
}