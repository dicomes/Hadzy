namespace SharedEventContracts;

public interface ICommentsFetchStatusEvent
{
    public string VideoId { get; }
    public string PageToken { get; }
    public int CommentsFetchedCount { get; }
    public int ReplyCount { get; }
}