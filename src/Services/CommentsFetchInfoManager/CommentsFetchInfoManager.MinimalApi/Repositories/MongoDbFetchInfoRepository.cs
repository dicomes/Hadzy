using CommentsFetchInfoManager.MinimalApi.Configurations;
using CommentsFetchInfoManager.MinimalApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsFetchInfoManager.MinimalApi.Repositories;

public class MongoDbFetchInfoRepository : IFetchInfoRepository
{
    private readonly IMongoCollection<VideoFetchInfo> _videoFetchInfo;

    public MongoDbFetchInfoRepository(IOptions<MongoDbConfig> mongoDbConfig)
    {
        var mongoClient = new MongoClient(mongoDbConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.Value.DatabaseName);
        _videoFetchInfo = mongoDatabase.GetCollection<VideoFetchInfo>(mongoDbConfig.Value.VideoFetchInfoCollectionName);
    }

    public async Task<VideoFetchInfo> GetByIdAsync(string videoId)
    {
        return await _videoFetchInfo.Find<VideoFetchInfo>(f => f.VideoId == videoId).FirstOrDefaultAsync();
    }

    public async Task AddAsync(VideoFetchInfo videoFetchInfo)
    {
        await _videoFetchInfo.InsertOneAsync(videoFetchInfo);
    }

    public async Task UpdateAsync(VideoFetchInfo videoFetchInfo)
    {
        var filter = Builders<VideoFetchInfo>.Filter.Eq(v => v.VideoId, videoFetchInfo.VideoId);
        await _videoFetchInfo.ReplaceOneAsync(filter, videoFetchInfo);
    }

    public async Task DeleteAsync(string videoId)
    {
        await _videoFetchInfo.DeleteOneAsync(videoId);
    }
}





