using IntegrationEventsContracts;

namespace CommentsFetchInfoIntegration.Worker.Services.Interfaces;

public interface IFetchInfoChangedEventHandler
{
    Task HandeAsync(IFetchInfoChangedEvent fetchInfoChangedEvent);
}