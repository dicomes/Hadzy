using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Contracts;

public interface ICommentFetchIterator
{
    Task<CommentThreadListCompletedEvent> Next(FetchParams fetchParams);
    public bool HasNext();
}