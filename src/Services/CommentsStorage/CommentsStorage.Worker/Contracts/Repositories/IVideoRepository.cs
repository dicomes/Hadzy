using CommentsStorage.Worker.Models;

namespace CommentsStorage.Worker.Contracts.Repositories;

public interface IVideoRepository
{
    Task<Video?> GetByIdAsync(string id, bool trackChanges);
    Task CreateVideo(Video video);
    Task UpdateVideo(Video video);
    Task DeleteVideo(Video video);
}