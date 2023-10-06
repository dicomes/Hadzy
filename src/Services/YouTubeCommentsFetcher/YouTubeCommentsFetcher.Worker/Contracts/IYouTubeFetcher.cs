using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Contracts;

public interface IYouTubeFetcher
{
    Task<CommentThreadListResponse> FetchAsync(FetchParams fetchParams);
}
