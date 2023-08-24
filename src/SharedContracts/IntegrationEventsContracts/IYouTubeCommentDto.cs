namespace IntegrationEventsContracts;

public interface IYouTubeCommentDto
{
    string Etag { get; }
    string Id { get; }
    string AuthorDisplayName { get; }
    string AuthorProfileImageUrl { get; }
    string AuthorChannelUrl { get; }
    string AuthorChannelId { get; }
    string ChannelId { get; }
    string VideoId { get; }
    string TextDisplay { get; }
    string TextOriginal { get; }
    string ParentId { get; }
    bool CanRate { get; }
    string ViewerRating { get; }
    ulong LikeCount { get; }
    string ModerationStatus { get; }
    DateTimeOffset PublishedAt { get; }
    DateTimeOffset UpdatedAt { get; }
    uint TotalReplyCount { get; }
}