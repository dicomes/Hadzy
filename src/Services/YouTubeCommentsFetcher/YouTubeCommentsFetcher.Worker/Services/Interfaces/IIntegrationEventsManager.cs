namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface IIntegrationEventsManager
{
    public Task FetchCommentsAndPublishFetchedEventsAsync(string videoId, string pageToken);
}