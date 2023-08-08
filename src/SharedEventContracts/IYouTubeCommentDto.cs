namespace SharedEventContracts;

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
    uint LikeCount { get; }
    string ModerationStatus { get; }
    DateTimeOffset PublishedAt { get; }
    DateTimeOffset UpdatedAt { get; }
    int TotalReplyCount { get; }
}