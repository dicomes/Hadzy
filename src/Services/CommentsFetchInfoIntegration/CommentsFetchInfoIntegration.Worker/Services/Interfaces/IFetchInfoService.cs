using CommentsFetchInfoIntegration.Worker.Models;

namespace CommentsFetchInfoIntegration.Worker.Services.Interfaces;

public interface IFetchInfoService
{
    Task<bool> FetchInfoByIdExistsAsync(string? videoId);
    Task UpdateFetchInfoAsync(string videoId, int newTotalCommentsFetched, string newStatus);
    Task UpdateFetchInfoAsync(VideoFetchInfo videoFetchInfo);
    Task<VideoFetchInfo> GetFetchInfoByIdAsync(string? videoId);
    Task InsertFetchInfoAsync(VideoFetchInfo videoFetchInfo);
}