namespace IntegrationEventsContracts;

public interface ICommentsFetchStatusEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string PageToken { get; }
    int CommentsFetchedCount { get; }
    int ReplyCount { get; }
    bool IsFetching { get; }
}