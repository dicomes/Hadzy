using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Contracts;

public interface ICommentsOverlapHandler
{
    OverlapResult HandleOverlaps(IList<CommentThread> items, List<string> storedCommentIds);
}
