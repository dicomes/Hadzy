using CommentsManager.Api.Models;

namespace CommentsManager.Api.Contracts.Services;

public interface IVideoService
{
    Task AddVideoAsync(Video video);
    Task<Video?> GetByIdAsync(string id, bool trackChanges);
    Task UpdateAsync(Video video);
    Task AddOrUpdateAsync(Video video);
    Task DeleteAsync(string id);
}