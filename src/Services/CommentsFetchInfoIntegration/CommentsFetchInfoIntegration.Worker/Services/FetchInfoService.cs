using CommentsFetchInfoIntegration.Worker.Configurations;
using CommentsFetchInfoIntegration.Worker.Models;
using CommentsFetchInfoIntegration.Worker.Repositories;
using CommentsFetchInfoIntegration.Worker.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsFetchInfoIntegration.Worker.Services;

public class FetchInfoService : IFetchInfoService
{
    private readonly IFetchInfoRepository _fetchInfoRepository;

    public FetchInfoService(
        IFetchInfoRepository fetchInfoRepository)
    {
        _fetchInfoRepository = fetchInfoRepository;
    }

    public async Task<bool> ExistsByIdAsync(string videoId)
    {
        var count = await _fetchInfoRepository.CountByIdAsync(videoId);
        return count > 0;
    }

    public async Task UpdateAsync(VideoFetchInfo videoFetchInfo)
    {
        await _fetchInfoRepository.UpdateAsync(videoFetchInfo);
    }
    
    public async Task<VideoFetchInfo> GetByIdAsync(string videoId)
    {
        return await _fetchInfoRepository.GetByIdAsync(videoId);
    }

    public async Task AddAsync(VideoFetchInfo videoFetchInfo)
    {
        await _fetchInfoRepository.AddAsync(videoFetchInfo);
    }
}