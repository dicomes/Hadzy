namespace SharedEventContracts;

public interface ICommentsFetchedEvent
{
    string VideoId { get; }
    public string PageToken { get; }
    public int CommentsCount { get; set; }
    public int TotalReplyCount { get; }
    List<IYouTubeCommentDto> YouTubeCommentsList { get; }
}