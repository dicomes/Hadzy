namespace IntegrationEventsContracts;

public interface IFetchInfoChangedEvent
{
    Guid Id { get; }
    string? VideoId { get; }
    string? PageToken { get; }
    ulong CommentsCount { get; }
    List<string>? CommentIds { get; }
    uint ReplyCount { get; }
    string? Status { get; }
    bool CompletedTillFirstComment { get; }
}