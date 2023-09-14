using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentFetchIterator
{
    Task<CommentThreadListCompletedEvent> Next(FetchParams fetchParams);
    public bool HasNext();
}