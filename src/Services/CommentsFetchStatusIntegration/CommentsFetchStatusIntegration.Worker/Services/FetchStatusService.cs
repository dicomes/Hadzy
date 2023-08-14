using CommentsFetchStatusIntegration.Worker.Configurations;
using CommentsFetchStatusIntegration.Worker.Configurations.Interfaces;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsFetchStatusIntegration.Worker.Services;

public class FetchStatusService : IFetchStatusService
{
    private readonly IMongoCollection<VideoFetchInfo> _fetchStatus;

    public FetchStatusService(
        IMongoDbConfig mongoDbConfig)
    {
        var mongoClient = new MongoClient(mongoDbConfig.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.DatabaseName);
        _fetchStatus = mongoDatabase.GetCollection<VideoFetchInfo>(mongoDbConfig.VideoFetchInfoCollectionName);
    }

    public async Task<bool> FetchStatusByIdExistsAsync(string videoId)
    {
        var filter = Builders<VideoFetchInfo>.Filter.Eq(x => x.VideoId, videoId);
        var count = await _fetchStatus.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task UpdateFetchStatusAsync(VideoFetchInfo videoFetchInfo)
    {
        await _fetchStatus.ReplaceOneAsync(x => x.VideoId == videoFetchInfo.VideoId, videoFetchInfo);
    }
    
    public async Task UpdateFetchStatusAsync(string videoId, int newTotalCommentsFetched, string newStatus)
    {
        var filter = Builders<VideoFetchInfo>.Filter.Eq(x => x.VideoId, videoId);
        var update = Builders<VideoFetchInfo>.Update
            .Set(x => x.CommentsCount, newTotalCommentsFetched)
            .Set(x => x.Status, newStatus);
    
        await _fetchStatus.UpdateOneAsync(filter, update);
    }

    public async Task<VideoFetchInfo> GetFetchStatusByIdAsync(string videoId)
    {
        return await _fetchStatus.Find(x => x.VideoId == videoId).FirstOrDefaultAsync();
    }

    public async Task InsertFetchStatusAsync(VideoFetchInfo videoFetchInfo)
    {
        await _fetchStatus.InsertOneAsync(videoFetchInfo);
    }
}