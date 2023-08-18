using IntegrationEventsContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class CommentThreadListCompletedEvent : IFetchCommentThreadListCompletedEvent
{
    public CommentThreadListCompletedEvent()
    {
        Id = Guid.NewGuid();
        CommentIds = new List<string>();
    }

    public Guid Id { get; }
    public string VideoId { get; set; }
    public string? NextPageToken { get; set; } 
    public int CommentsCount { get; set; }
    public int ReplyCount { get; set; }
    public List<string> CommentIds { get; set; }
    public List<IYouTubeCommentDto> YouTubeCommentsList { get; set; }

    public override string ToString() =>
        $"FetchBatchCompletedEvent - Guid {Id} .VideoId: {VideoId}, NextPageToken: {NextPageToken}, " +
        $"CommentsCount: {CommentsCount}, ReplyCount: {ReplyCount}";
}