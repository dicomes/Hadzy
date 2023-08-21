using CommentsFetchInfoIntegration.Worker.IntegrationEvents;

namespace CommentsFetchInfoIntegration.Worker.Services.Interfaces;

public interface IFetchInfoChangedEventHandler
{
    Task HandeAsync(FetchInfoChangedEvent fetchInfoChangedEvent);
}