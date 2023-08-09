namespace IntegrationEventsContracts;

public interface IFetchStatusChangedEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string PageToken { get; }
    int CommentsFetchedCount { get; }
    int ReplyCount { get; }
    bool IsFetching { get; }
}