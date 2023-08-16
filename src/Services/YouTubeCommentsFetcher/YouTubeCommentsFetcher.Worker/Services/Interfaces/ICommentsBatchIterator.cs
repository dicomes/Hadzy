using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentsBatchIterator
{
    Task<FetchBatchCompletedEvent> Next(FetchSettings fetchSettings);
    public bool HasNext();
}