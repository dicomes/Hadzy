using Google.Apis.YouTube.v3.Data;

namespace YouTubeCommentsFetcher.Worker.Models;

public class OverlapResult
{
    public IList<CommentThread> UpdatedCommentList { get; set; }
    public int OverlappingCount { get; set; }
    public int BaseCommentCount { get; set; }
    public bool ShouldStopFetching { get; set; }
}