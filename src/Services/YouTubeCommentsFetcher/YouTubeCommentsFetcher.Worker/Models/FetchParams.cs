namespace YouTubeCommentsFetcher.Worker.Models;

public class FetchParams
{
    public string VideoId { get; }
    public string? PageToken { get; set; }
    public List<string> FirstCommentsFromPreviousBatch { get; }

    public FetchParams(string videoId, string? pageToken, List<string>? commentIds)
    {
        VideoId = videoId;
        PageToken = pageToken ?? string.Empty;
        FirstCommentsFromPreviousBatch = commentIds ?? new List<string>();
    }
}