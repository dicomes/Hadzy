namespace YouTubeCommentsFetcher.Worker.Contracts;

public interface IEventsManager
{
    Task IterateCommentsAndPublishEventsAsync(string? videoId, string? pageToken, List<string> commentIds);
}