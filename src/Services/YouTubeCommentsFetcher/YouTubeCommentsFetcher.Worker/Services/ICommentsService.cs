using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services;

public interface ICommentsService
{
    Task<CommentsFetchedEvent> GetCommentsFetchedEventByVideoIdAsync(FetchSettings fetchSettings);
}