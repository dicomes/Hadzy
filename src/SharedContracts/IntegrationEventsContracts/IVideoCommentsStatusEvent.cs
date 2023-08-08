namespace IntegrationEventsContracts;

public interface IVideoCommentsStatusEvent
{
    Guid Id { get; }
    string VideoId { get; }
    int TotalCommentsFetched { get; }
    bool IsFetching { get; }
}