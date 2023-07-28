using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public interface ICommentsFetchedEvent
{
    string VideoId { get; }
    List<YouTubeComment> YouTubeCommentsList { get; }
}