using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services.Fetcher;

public interface IFetcherService
{
    Task<CommentThreadListResponse> FetchAsync(FetchSettings fetchSettings);
}
