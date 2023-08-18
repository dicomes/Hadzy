namespace YouTubeCommentsFetcher.Worker.Models;

public class FetchParams
{
    public readonly string Properties = "snippet";
    public readonly int MaxResults = 100;
    public string VideoId { get; }
    public string PageToken { get; set; }
    public readonly List<string> PreviouslyFetchedCommentIds;

    public FetchParams(string videoId, string? pageToken, List<string> commentIds)
    {
        VideoId = videoId;
        PageToken = !string.IsNullOrEmpty(pageToken) ? pageToken : string.Empty;
        PreviouslyFetchedCommentIds = (commentIds == null) ? new List<string>() : commentIds;
    }
}