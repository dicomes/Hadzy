using SharedEventContracts;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentsFetchedEventPublisher
{
    Task PublishFetchedEvent(ICommentsFetchedEvent commentsFetchedEvent);
}