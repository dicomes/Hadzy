using CommentsFetchStatusIntegration.Worker.Models;

namespace CommentsFetchStatusIntegration.Worker.Services.Interfaces;

public interface IFetchStatusService
{
    Task<bool> FetchStatusByIdExistsAsync(string videoId);
    Task UpdateFetchStatusAsync(string videoId, int newTotalCommentsFetched, bool isFetching);
    Task UpdateFetchStatusAsync(VideoFetchStatus videoFetchStatus);
    Task<VideoFetchStatus> GetFetchStatusByIdAsync(string videoId);
    Task InsertFetchStatusAsync(VideoFetchStatus videoFetchStatus);
}