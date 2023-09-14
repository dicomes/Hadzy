namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentPublisher
{
    Task IterateAndPublishCommentsAsync(string? videoId, string? pageToken, List<string> commentIds);
}