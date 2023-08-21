using CommentsFetchInfoManager.MinimalApi.Configurations;
using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class FetchInfoRepository : IFetchInfoRepository
{
    private readonly IMongoCollection<VideoFetchInfo> _videoFetchInfo;

    public FetchInfoRepository(IOptions<MongoDbConfig> mongoDbConfig)
    {
        var mongoClient = new MongoClient(mongoDbConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.Value.DatabaseName);
        _videoFetchInfo = mongoDatabase.GetCollection<VideoFetchInfo>(mongoDbConfig.Value.VideoFetchInfoCollectionName);
    }

    public async Task<VideoFetchInfo> GetByVideoId(string? videoId)
    {
        return await _videoFetchInfo.Find<VideoFetchInfo>(f => f.VideoId == videoId).FirstOrDefaultAsync();
    }

    public async Task Add(VideoFetchInfo videoFetchInfo)
    {
        await _videoFetchInfo.InsertOneAsync(videoFetchInfo);
    }

    public async Task Update(VideoFetchInfo videoFetchInfo)
    {
        var filter = Builders<VideoFetchInfo>.Filter.Eq(v => v.VideoId, videoFetchInfo.VideoId);
        await _videoFetchInfo.ReplaceOneAsync(filter, videoFetchInfo);
    }

}





