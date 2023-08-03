using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface IYouTubeCommentsManager
{
    Task<ICommentsFetchedEvent> FetchAndTransformCommentsAsync(FetchSettings fetchSettings);
}