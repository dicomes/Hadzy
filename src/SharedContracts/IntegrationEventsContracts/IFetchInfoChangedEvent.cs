namespace IntegrationEventsContracts;

public interface IFetchInfoChangedEvent
{
    Guid Id { get; }
    string? VideoId { get; }
    string? PageToken { get; }
    int CommentsCount { get; }
    List<string>? CommentIds { get; }
    int ReplyCount { get; }
    string? Status { get; }
    bool CompletedTillFirstComment { get; }
}