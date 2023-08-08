using CommentsFetchStatusIntegration.Worker.Configurations;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CommentsFetchStatusIntegration.Worker.Services;

public class VideoCommentsStatusService : IVideoCommentsStatusService
{
    private readonly IMongoCollection<VideoCommentsStatus> _videoCommentsCollection;

    public VideoCommentsStatusService(IOptions<MongoDbConfig> mongoDbConfig)
    {
        var mongoClient = new MongoClient(mongoDbConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.Value.DatabaseName);
        _videoCommentsCollection = mongoDatabase.GetCollection<VideoCommentsStatus>(mongoDbConfig.Value.CommentsStatusCollectionName);
    }

    public async Task<bool> VideoIdExistsAsync(string videoId)
    {
        var filter = Builders<VideoCommentsStatus>.Filter.Eq(x => x.VideoId, videoId);
        var count = await _videoCommentsCollection.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task UpdateVideoCommentsStatusAsync(VideoCommentsStatus videoCommentsStatus)
    {
        await _videoCommentsCollection.ReplaceOneAsync(x => x.VideoId == videoCommentsStatus.VideoId, videoCommentsStatus);
    }

    public async Task<VideoCommentsStatus> GetVideoCommentsStatusByVideoIdAsync(string videoId)
    {
        return await _videoCommentsCollection.Find(x => x.VideoId == videoId).FirstOrDefaultAsync();
    }

    public async Task InsertVideoCommentsStatusAsync(VideoCommentsStatus videoCommentsStatus)
    {
        await _videoCommentsCollection.InsertOneAsync(videoCommentsStatus);
    }
}