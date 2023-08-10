using CommentsFetchStatusIntegration.Worker.Models;

namespace CommentsFetchStatusIntegration.Worker.Services.Interfaces;

public interface IFetchStatusService
{
    Task<bool> VideoIdExistsAsync(string videoId);
    Task UpdateFetchStatusAsync(string videoId, int newTotalCommentsFetched, bool isFetching);
    Task UpdateFetchStatusAsync(FetchStatus fetchStatus);
    Task<FetchStatus> GetFetchStatusByVideoIdAsync(string videoId);
    Task InsertFetchStatusAsync(FetchStatus fetchStatus);
}