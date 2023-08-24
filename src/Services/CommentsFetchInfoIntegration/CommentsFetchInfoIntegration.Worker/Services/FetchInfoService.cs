using CommentsFetchInfoIntegration.Worker.Configurations;
using CommentsFetchInfoIntegration.Worker.Configurations.Interfaces;
using CommentsFetchInfoIntegration.Worker.Models;
using CommentsFetchInfoIntegration.Worker.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsFetchInfoIntegration.Worker.Services;

public class FetchInfoService : IFetchInfoService
{
    private readonly IMongoCollection<VideoFetchInfo> _fetchStatus;

    public FetchInfoService(
        IMongoDbConfig mongoDbConfig)
    {
        var mongoClient = new MongoClient(mongoDbConfig.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.DatabaseName);
        _fetchStatus = mongoDatabase.GetCollection<VideoFetchInfo>(mongoDbConfig.VideoFetchInfoCollectionName);
    }

    public async Task<bool> FetchInfoByIdExistsAsync(string? videoId)
    {
        var filter = Builders<VideoFetchInfo>.Filter.Eq(x => x.VideoId, videoId);
        var count = await _fetchStatus.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task UpdateFetchInfoAsync(VideoFetchInfo videoFetchInfo)
    {
        await _fetchStatus.ReplaceOneAsync(x => x.VideoId == videoFetchInfo.VideoId, videoFetchInfo);
    }
    
    public async Task UpdateFetchInfoAsync(string videoId, ulong newTotalCommentsFetched, string newStatus)
    {
        var filter = Builders<VideoFetchInfo>.Filter.Eq(x => x.VideoId, videoId);
        var update = Builders<VideoFetchInfo>.Update
            .Set(x => x.CommentsCount, newTotalCommentsFetched)
            .Set(x => x.Status, newStatus);
    
        await _fetchStatus.UpdateOneAsync(filter, update);
    }

    public async Task<VideoFetchInfo> GetFetchInfoByIdAsync(string? videoId)
    {
        return await _fetchStatus.Find(x => x.VideoId == videoId).FirstOrDefaultAsync();
    }

    public async Task InsertFetchInfoAsync(VideoFetchInfo videoFetchInfo)
    {
        await _fetchStatus.InsertOneAsync(videoFetchInfo);
    }
}