using CommentsFetchInfoIntegration.Worker.Models;

namespace CommentsFetchInfoIntegration.Worker.Services.Interfaces;

public interface IFetchInfoService
{
    Task<bool> ExistsByIdAsync(string videoId);
    Task UpdateAsync(VideoFetchInfo videoFetchInfo);
    Task<VideoFetchInfo> GetByIdAsync(string videoId);
    Task AddAsync(VideoFetchInfo videoFetchInfo);
}