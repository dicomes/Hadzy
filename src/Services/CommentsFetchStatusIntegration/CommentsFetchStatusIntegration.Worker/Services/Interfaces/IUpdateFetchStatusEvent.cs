using CommentsFetchStatusIntegration.Worker.IntegrationEvents;

namespace CommentsFetchStatusIntegration.Worker.Services.Interfaces;

public interface IUpdateFetchStatusEvent
{
    Task UpdateAsync(FetchStatusChangedEvent fetchStatusChangedEvent);
}