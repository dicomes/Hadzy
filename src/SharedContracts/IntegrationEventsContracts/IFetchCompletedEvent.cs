namespace IntegrationEventsContracts;

public interface IFetchCompletedEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string? FirstCommentId { get; }
}