using CommentsFetchStatusIntegration.Worker.Configurations;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsFetchStatusIntegration.Worker.Services;

public class FetchStatusService : IFetchStatusService
{
    private readonly IMongoCollection<FetchStatus> _videoCommentsCollection;

    public FetchStatusService(IOptions<MongoDbConfig> mongoDbConfig)
    {
        var mongoClient = new MongoClient(mongoDbConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.Value.DatabaseName);
        _videoCommentsCollection = mongoDatabase.GetCollection<FetchStatus>(mongoDbConfig.Value.CollectionName);
    }

    public async Task<bool> VideoIdExistsAsync(string videoId)
    {
        var filter = Builders<FetchStatus>.Filter.Eq(x => x.VideoId, videoId);
        var count = await _videoCommentsCollection.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task UpdateFetchStatusAsync(FetchStatus fetchStatus)
    {
        await _videoCommentsCollection.ReplaceOneAsync(x => x.VideoId == fetchStatus.VideoId, fetchStatus);
    }
    
    public async Task UpdateFetchStatusAsync(string videoId, int newTotalCommentsFetched, bool isFetching)
    {
        var filter = Builders<FetchStatus>.Filter.Eq(x => x.VideoId, videoId);
        var update = Builders<FetchStatus>.Update
            .Set(x => x.TotalCommentsFetched, newTotalCommentsFetched)
            .Set(x => x.IsFetching, isFetching);
    
        await _videoCommentsCollection.UpdateOneAsync(filter, update);
    }

    public async Task<FetchStatus> GetFetchStatusByVideoIdAsync(string videoId)
    {
        return await _videoCommentsCollection.Find(x => x.VideoId == videoId).FirstOrDefaultAsync();
    }

    public async Task InsertFetchStatusAsync(FetchStatus fetchStatus)
    {
        await _videoCommentsCollection.InsertOneAsync(fetchStatus);
    }
}