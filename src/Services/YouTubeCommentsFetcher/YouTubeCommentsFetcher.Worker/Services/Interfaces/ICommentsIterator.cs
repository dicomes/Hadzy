using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentsIterator
{
    Task<FetchCompletedEvent> Next(FetchSettings fetchSettings);
    public bool HasNext();
}