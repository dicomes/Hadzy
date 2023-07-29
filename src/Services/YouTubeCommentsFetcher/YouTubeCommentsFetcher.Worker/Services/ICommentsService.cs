using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services;

public interface ICommentsService
{
    Task<ICommentsFetchedEvent> GetCommentsFetchedEventByVideoIdAsync(FetchSettings fetchSettings);
}