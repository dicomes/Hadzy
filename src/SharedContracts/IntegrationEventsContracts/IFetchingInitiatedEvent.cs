namespace IntegrationEventsContracts;

public interface IFetchingInitiatedEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string PageToken { get; }
}