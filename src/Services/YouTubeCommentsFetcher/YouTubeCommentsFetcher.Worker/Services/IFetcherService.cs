using Google.Apis.YouTube.v3.Data;

namespace YouTubeCommentsFetcher.Worker.Services;

public interface IFetcherService
{
    Task<CommentThreadListResponse> GetCommentThreadList(string videoId, int maxResults);
}
