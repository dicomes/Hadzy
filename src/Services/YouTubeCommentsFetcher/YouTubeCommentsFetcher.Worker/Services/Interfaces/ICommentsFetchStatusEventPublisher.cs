using SharedEventContracts;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentsFetchStatusEventPublisher
{
    Task PublishFetchStatusEvent(ICommentsFetchStatusEvent commentsFetchStatusEvent);
}