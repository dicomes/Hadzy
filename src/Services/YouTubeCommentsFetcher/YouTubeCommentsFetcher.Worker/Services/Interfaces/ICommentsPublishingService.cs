using SharedEventContracts;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentsPublishingService
{
    Task PublishCommentsFetchedEventAsync(ICommentsFetchedEvent commentsFetchedEvent);
}