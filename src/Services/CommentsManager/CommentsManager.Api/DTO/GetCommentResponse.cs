namespace CommentsManager.Api.DTO;

public class GetCommentResponse
{
    public string Id { get; set; }
    public string AuthorDisplayName { get; set; }
    public string AuthorProfileImageUrl { get; set; }
    public string AuthorChannelUrl { get; set; }
    public string AuthorChannelId { get; set; }
    public string ChannelId { get; set; }
    public string VideoId { get; set; }
    public string TextDisplay { get; set; }
    public ulong LikeCount { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public uint TotalReplyCount { get; set; }
}