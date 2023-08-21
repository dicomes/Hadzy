using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentThreadIterator
{
    Task<CommentThreadListCompletedEvent> Next(FetchParams fetchParams);
    public bool HasNext();
}