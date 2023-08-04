namespace SharedEventContracts;

public interface ICommentsFetchedEvent
{
    string VideoId { get; }
    public string PageToken { get; }
    public int CommentsFetchedCount { get; set; }
    public int ReplyCount { get; }
    List<IYouTubeCommentDto> YouTubeCommentsList { get; }
}