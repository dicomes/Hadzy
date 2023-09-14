namespace EFContracts;

public interface IComment
{
    public string Etag { get; }
    public string Id { get; }
    public string AuthorDisplayName { get; }
    public string AuthorProfileImageUrl { get; }
    public string AuthorChannelUrl { get; }
    public string AuthorChannelId { get; }
    public string ChannelId { get; }
    public string VideoId { get;}
    public string TextDisplay { get; }
    public string TextOriginal { get; }
    public string ViewerRating { get; }
    public ulong LikeCount { get; }
    public DateTimeOffset PublishedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    public uint TotalReplyCount { get; }
}