namespace IntegrationEventsContracts;

public interface ICommentsFetchReceivedEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string PageToken { get; }
}