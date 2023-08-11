using CommentsFetchStatusIntegration.Worker.Configurations;
using CommentsFetchStatusIntegration.Worker.Configurations.Interfaces;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsFetchStatusIntegration.Worker.Services;

public class FetchStatusService : IFetchStatusService
{
    private readonly IMongoCollection<VideoFetchStatus> _fetchStatus;

    public FetchStatusService(
        IMongoDbConfig mongoDbConfig)
    {
        var mongoClient = new MongoClient(mongoDbConfig.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.DatabaseName);
        _fetchStatus = mongoDatabase.GetCollection<VideoFetchStatus>(mongoDbConfig.CommentsFetchStatusCollectionName);
    }

    public async Task<bool> FetchStatusByIdExistsAsync(string videoId)
    {
        var filter = Builders<VideoFetchStatus>.Filter.Eq(x => x.VideoId, videoId);
        var count = await _fetchStatus.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task UpdateFetchStatusAsync(VideoFetchStatus videoFetchStatus)
    {
        await _fetchStatus.ReplaceOneAsync(x => x.VideoId == videoFetchStatus.VideoId, videoFetchStatus);
    }
    
    public async Task UpdateFetchStatusAsync(string videoId, int newTotalCommentsFetched, bool isFetching)
    {
        var filter = Builders<VideoFetchStatus>.Filter.Eq(x => x.VideoId, videoId);
        var update = Builders<VideoFetchStatus>.Update
            .Set(x => x.TotalCommentsProcessed, newTotalCommentsFetched)
            .Set(x => x.IsFetching, isFetching);
    
        await _fetchStatus.UpdateOneAsync(filter, update);
    }

    public async Task<VideoFetchStatus> GetFetchStatusByIdAsync(string videoId)
    {
        return await _fetchStatus.Find(x => x.VideoId == videoId).FirstOrDefaultAsync();
    }

    public async Task InsertFetchStatusAsync(VideoFetchStatus videoFetchStatus)
    {
        await _fetchStatus.InsertOneAsync(videoFetchStatus);
    }
}