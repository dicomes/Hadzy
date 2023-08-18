using Google.Apis.YouTube.v3.Data;

namespace YouTubeCommentsFetcher.Worker.Models;

public class OverlapResult
{
    public CommentThreadListResponse UpdatedResponse { get; set; }
    public int OverlappingCount { get; set; }
    public bool ShouldStopFetching { get; set; }
}