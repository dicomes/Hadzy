namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface IFetchEventsIntegration
{
    public Task FetchAndPublishAsync(string videoId, string pageToken);
}