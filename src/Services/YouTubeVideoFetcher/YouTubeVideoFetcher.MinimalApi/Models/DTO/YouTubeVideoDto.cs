namespace YouTubeVideoFetcher.MinimalApi.Models.DTO;

public class YouTubeVideoDto
{
    public string id { get; set; }
    public DateTime publishedAt { get; set; }
    public string title { get; set; }
    public string channelTitle { get; set; }
    public ulong? viewCount { get; set; }
    public ulong? likeCount { get; set; }
    public ulong? commentCount { get; set; }
    public string channelId { get; set; }
}
