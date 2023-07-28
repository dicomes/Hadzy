namespace YouTubeCommentsFetcher.Worker.Models;

public class FetchSettings
{
    public string VideoId { get; set; }
    public string PageToken { get; set; }
    public string Properties { get; set; } 
    public int MaxResults { get; set; }
}