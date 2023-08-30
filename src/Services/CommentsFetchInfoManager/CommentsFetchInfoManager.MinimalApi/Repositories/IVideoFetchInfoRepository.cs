using CommentsFetchInfoManager.MinimalApi.Models;

namespace CommentsFetchInfoManager.MinimalApi.Repositories;

public interface IVideoFetchInfoRepository
{
    Task<VideoFetchInfo> GetByIdAsync(string videoId);
    Task AddAsync(VideoFetchInfo videoFetchInfo);
    Task UpdateAsync(VideoFetchInfo videoFetchInfo);
    Task DeleteAsync(string videoId);
}