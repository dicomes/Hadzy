using CommentsStorage.Worker.Contracts.Repositories;
using CommentsStorage.Worker.Contracts.Services;
using CommentsStorage.Worker.Models;

namespace CommentsStorage.Worker.Services;

public class VideoService : IVideoService
{
    private readonly IRepositoryManager _repository;

    public VideoService(IRepositoryManager repository)
    {
        _repository = repository;
    }
    
    public async Task<Video?> GetByIdAsync(string id, bool trackChanges)
    {
        return await _repository.Video.GetByIdAsync(id, false);
    }
    
    public async Task AddAsync(Video video)
    {
        var existentVideo  = await _repository.Video.GetByIdAsync(video.Id, false);
        if (existentVideo == null)
        {
            await _repository.Video.CreateVideo(video);
        }
    }
    
    public async Task UpdateAsync(Video video)
    {
        var existentVideo  = await _repository.Video.GetByIdAsync(video.Id, false);
        if (existentVideo != null)
        {
            await _repository.Video.UpdateVideo(video);
        }
    }

    public async Task DeleteByIdAsync(string id)
    {
        var video = await _repository.Video.GetByIdAsync(id, false);
        if (video != null)
        {
            await _repository.Video.DeleteVideo(video);
        }
    }
}