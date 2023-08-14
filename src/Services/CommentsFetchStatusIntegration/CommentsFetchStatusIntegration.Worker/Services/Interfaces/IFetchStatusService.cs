using CommentsFetchStatusIntegration.Worker.Models;

namespace CommentsFetchStatusIntegration.Worker.Services.Interfaces;

public interface IFetchStatusService
{
    Task<bool> FetchStatusByIdExistsAsync(string videoId);
    Task UpdateFetchStatusAsync(string videoId, int newTotalCommentsFetched, string newStatus);
    Task UpdateFetchStatusAsync(VideoFetchInfo videoFetchInfo);
    Task<VideoFetchInfo> GetFetchStatusByIdAsync(string videoId);
    Task InsertFetchStatusAsync(VideoFetchInfo videoFetchInfo);
}