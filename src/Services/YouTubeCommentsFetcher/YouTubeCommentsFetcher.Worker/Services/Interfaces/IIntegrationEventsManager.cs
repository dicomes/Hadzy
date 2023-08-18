namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface IIntegrationEventsManager
{
    Task ProcessCommentsAndPublishFetchedEventsAsync(string videoId, string? pageToken, List<string> commentIds);
}