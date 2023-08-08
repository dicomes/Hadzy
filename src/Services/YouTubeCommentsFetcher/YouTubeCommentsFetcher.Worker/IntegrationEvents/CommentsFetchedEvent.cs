using SharedEventContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class CommentsFetchedEvent : ICommentsFetchedEvent
{
    public Guid Id { get; set; }
    public string VideoId { get; set; }
    public string PageToken { get; set; } 
    public int CommentsFetchedCount { get; set; }
    public int ReplyCount { get; set; }
    public List<IYouTubeCommentDto> YouTubeCommentsList { get; set; }

    public override string ToString() =>
        $"CommentsFetchedEvent - Guid {Id} .VideoId: {VideoId}, PageToken: {PageToken}, " +
        $"CommentsFetchedCount: {CommentsFetchedCount}, ReplyCount: {ReplyCount}";
}