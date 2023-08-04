using SharedEventContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class CommentsFetchedStatusEvent : ICommentsFetchStatusEvent
{
    public string VideoId { get; set; }
    public string PageToken { get; set; }
    public int CommentsFetchedCount { get; set; }
    public int ReplyCount { get; set; }
}