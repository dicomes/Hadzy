namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentsIntegrationOrchestrator
{
    public Task ProcessCommentsForVideoAsync(string videoId, string nextPageToken);
}