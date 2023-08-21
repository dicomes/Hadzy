namespace IntegrationEventsContracts;

public interface IFetchStartedEvent
{
    Guid Id { get; }
    string? VideoId { get; }
    string PageToken { get; }
    List<string> FirstFetchedCommentIds { get; }
}