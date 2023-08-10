using CommentsFetchStatusIntegration.Worker.IntegrationEvents;

namespace CommentsFetchStatusIntegration.Worker.Services.Interfaces;

public interface IIntegrateFetchStatusEvent
{
    Task UpdateAsync(FetchStatusChangedEvent fetchStatusChangedEvent);
}