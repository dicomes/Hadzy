namespace CommentsStorage.Worker.Models;

public class Comment
{
    public string Etag { get; set; }
    public string Id { get; set; }
    public string AuthorDisplayName { get; set; }
    public string AuthorProfileImageUrl { get; set; }
    public string AuthorChannelUrl { get; set; }
    public string AuthorChannelId { get; set; }
    public string ChannelId { get; set; }
    public string VideoId { get; set; }
    public string TextDisplay { get; set; }
    public string TextOriginal { get; set; }
    public string ViewerRating { get; set; }
    public ulong LikeCount { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public uint TotalReplyCount { get; set; }

    public override string ToString() =>
        $"Comment - Id: {Id}." +
        $" VideoId: {VideoId}," +
        $" AuthorDisplayName: {AuthorDisplayName}";
}