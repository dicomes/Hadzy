namespace YouTubeCommentsFetcher.Worker.Models;

public class FetchParams
{
    public readonly string Properties = "snippet";
    public readonly int MaxResults = 100;
    public readonly string? VideoId;
    public string? PageToken { get; set; }
    public readonly List<string> FirstCommentsFromPreviousBatch;

    public FetchParams(string? videoId, string? pageToken, List<string> commentIds)
    {
        VideoId = videoId;
        PageToken = !string.IsNullOrEmpty(pageToken) ? pageToken : string.Empty;
        FirstCommentsFromPreviousBatch = (commentIds == null) ? new List<string>() : commentIds;
    }
    
    public FetchParams(string? videoId)
    {
        VideoId = videoId;
    }
}