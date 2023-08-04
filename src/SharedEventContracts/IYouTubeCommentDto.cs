namespace SharedEventContracts;

public interface IYouTubeCommentDto
{
    public string Etag { get; }
    public string Id { get; }
    public string AuthorDisplayName { get; }
    public string AuthorProfileImageUrl { get; }
    public string AuthorChannelUrl { get; }
    public string AuthorChannelId { get; }
    public string ChannelId { get; }
    public string VideoId { get; }
    public string TextDisplay { get; }
    public string TextOriginal { get; }
    public string ParentId { get; }
    public bool CanRate { get; }
    public string ViewerRating { get; }
    public uint LikeCount { get; }
    public string ModerationStatus { get; }
    public DateTimeOffset PublishedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    public int TotalReplyCount { get; }
}