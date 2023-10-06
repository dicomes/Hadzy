namespace IntegrationEventsContracts;

public interface ICommentThreadListCompletedEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string? NextPageToken { get; }
    ulong CommentsCount { get; }
    uint ReplyCount { get; }
    string? FirstCommentId { get; }
    List<string>? CommentIds { get; }
    List<IYouTubeCommentDto>? YouTubeCommentsList { get; }
}