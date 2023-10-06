using CommentsStorage.Worker.Models;

namespace CommentsStorage.Worker.Contracts.Services;

public interface IVideoService
{
    Task AddAsync(Video video);
    Task<Video?> GetByIdAsync(string id, bool trackChanges);
    Task UpdateAsync(Video video);
    Task DeleteByIdAsync(string id);
}