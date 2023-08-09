using YouTubeCommentsFetcher.Worker.IntegrationEvents;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentsIterator
{
    Task<FetchCompletedEvent> Next();
    public bool HasNext();
}