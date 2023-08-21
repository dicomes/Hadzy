namespace IntegrationEventsContracts;

public interface IFetchCommentThreadListCompletedEvent
{
    Guid Id { get; }
    string? VideoId { get; }
    string NextPageToken { get; }
    int CommentsCount { get; }
    int ReplyCount { get; }
    List<string>? CommentIds { get; }
    List<IYouTubeCommentDto> YouTubeCommentsList { get; }
}