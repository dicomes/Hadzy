using CommentsFetchInfoIntegration.Worker.Models;

namespace CommentsFetchInfoIntegration.Worker.Repositories;

public interface IFetchInfoRepository
{
    Task<VideoFetchInfo> GetByIdAsync(string videoId);
    Task AddAsync(VideoFetchInfo videoFetchInfo);
    Task UpdateAsync(VideoFetchInfo videoFetchInfo);
    Task DeleteAsync(string videoId);
    Task<long> CountByIdAsync(string videoId);
}