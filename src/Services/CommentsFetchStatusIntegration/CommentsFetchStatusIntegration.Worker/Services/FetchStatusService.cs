using CommentsFetchStatusIntegration.Worker.Configurations;
using CommentsFetchStatusIntegration.Worker.Configurations.Interfaces;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsFetchStatusIntegration.Worker.Services;

public class FetchStatusService : IFetchStatusService
{
    private readonly IMongoCollection<FetchStatus> _fetchStatus;

    public FetchStatusService(
        IMongoDbConfig mongoDbConfig)
    {
        var mongoClient = new MongoClient(mongoDbConfig.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.DatabaseName);
        _fetchStatus = mongoDatabase.GetCollection<FetchStatus>(mongoDbConfig.CommentsFetchStatusCollectionName);
    }

    public async Task<bool> VideoIdExistsAsync(string videoId)
    {
        var filter = Builders<FetchStatus>.Filter.Eq(x => x.VideoId, videoId);
        var count = await _fetchStatus.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task UpdateFetchStatusAsync(FetchStatus fetchStatus)
    {
        await _fetchStatus.ReplaceOneAsync(x => x.VideoId == fetchStatus.VideoId, fetchStatus);
    }
    
    public async Task UpdateFetchStatusAsync(string videoId, int newTotalCommentsFetched, bool isFetching)
    {
        var filter = Builders<FetchStatus>.Filter.Eq(x => x.VideoId, videoId);
        var update = Builders<FetchStatus>.Update
            .Set(x => x.TotalCommentsFetched, newTotalCommentsFetched)
            .Set(x => x.IsFetching, isFetching);
    
        await _fetchStatus.UpdateOneAsync(filter, update);
    }

    public async Task<FetchStatus> GetFetchStatusByVideoIdAsync(string videoId)
    {
        return await _fetchStatus.Find(x => x.VideoId == videoId).FirstOrDefaultAsync();
    }

    public async Task InsertFetchStatusAsync(FetchStatus fetchStatus)
    {
        await _fetchStatus.InsertOneAsync(fetchStatus);
    }
}