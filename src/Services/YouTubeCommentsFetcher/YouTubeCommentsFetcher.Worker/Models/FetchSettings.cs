namespace YouTubeCommentsFetcher.Worker.Models;

public class FetchSettings
{
    public readonly string Properties = "snippet";
    public readonly int MaxResults = 100;
    public string VideoId { get; set; }
    public string PageToken { get; set; }

    public FetchSettings(string videoId, string pageToken)
    {
        VideoId = videoId;
        PageToken = pageToken;
    }
}