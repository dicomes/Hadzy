using SharedEventContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class CommentsFetchedEvent : ICommentsFetchedEvent
{
    public string VideoId { get; set; }
    public string PageToken { get; set; } 
    public int CommentsCount { get; set; }
    public int TotalReplyCount { get; set; }
    public List<IYouTubeCommentDto> YouTubeCommentsList { get; set; }
}