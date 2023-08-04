using YouTubeCommentsFetcher.Worker.IntegrationEvents;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces
{
    public interface IErrorEventPublisher
    {
        Task PublishErrorEvent(IFetcherErrorEvent errorEvent);
    }
}