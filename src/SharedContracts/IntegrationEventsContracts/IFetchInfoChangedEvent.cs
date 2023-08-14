namespace IntegrationEventsContracts;

public interface IFetchInfoChangedEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string PageToken { get; }
    int CommentsCount { get; }
    int ReplyCount { get; }
    string Status { get; }
}