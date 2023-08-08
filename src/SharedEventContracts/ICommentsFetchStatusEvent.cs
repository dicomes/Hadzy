namespace SharedEventContracts;

public interface ICommentsFetchStatusEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string PageToken { get; }
    int CommentsFetchedCount { get; }
    int ReplyCount { get; }
}