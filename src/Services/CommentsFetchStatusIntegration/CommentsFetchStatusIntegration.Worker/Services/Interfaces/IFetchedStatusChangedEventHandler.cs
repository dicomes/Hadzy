using CommentsFetchStatusIntegration.Worker.IntegrationEvents;

namespace CommentsFetchStatusIntegration.Worker.Services.Interfaces;

public interface IFetchedStatusChangedEventHandler
{
    Task HandeAsync(FetchStatusChangedEvent fetchStatusChangedEvent);
}