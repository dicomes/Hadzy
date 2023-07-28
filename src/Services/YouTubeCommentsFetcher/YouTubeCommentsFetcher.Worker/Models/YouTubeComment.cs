namespace YouTubeCommentsFetcher.Worker.Models;

public class YouTubeComment
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
    public string ParentId { get; set; }
    public bool CanRate { get; set; }
    public string ViewerRating { get; set; }
    public uint LikeCount { get; set; }
    public string ModerationStatus { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}