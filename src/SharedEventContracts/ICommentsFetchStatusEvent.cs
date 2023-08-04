namespace SharedEventContracts;

public interface ICommentsFetchStatusEvent
{
    public string VideoId { get; set; }
    public int CommentsFetchedCount { get; set; }
    public int ReplyCount { get; set; }
}