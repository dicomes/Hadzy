using IntegrationEventsContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class CommentThreadListCompletedEvent : ICommentThreadListCompletedEvent
{
    public CommentThreadListCompletedEvent(string videoId)
    {
        Id = Guid.NewGuid();
        VideoId = videoId;
        CommentIds = new List<string>();
    }

    public Guid Id { get; }
    public string VideoId { get; set; }
    public string? NextPageToken { get; set; } 
    public ulong CommentsCount { get; set; }
    public uint ReplyCount { get; set; }
    public string? FirstCommentId { get; set; }
    public List<string>? CommentIds { get; set; }
    public List<IYouTubeCommentDto>? YouTubeCommentsList { get; set; }

    public override string ToString() =>
        $"FetchBatchCompletedEvent - Guid {Id} .VideoId: {VideoId}, NextPageToken: {NextPageToken}, " +
        $"CommentsCount: {CommentsCount}, ReplyCount: {ReplyCount}";
}