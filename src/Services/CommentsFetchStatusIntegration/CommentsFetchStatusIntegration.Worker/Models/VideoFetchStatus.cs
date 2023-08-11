using MongoDB.Bson.Serialization.Attributes;
using MongoDbContracts;

namespace CommentsFetchStatusIntegration.Worker.Models;

public class VideoFetchStatus : IVideoFetchStatus
{
    [BsonId]
    public string VideoId { get; set; }
    
    [BsonElement("TotalCommentsProcessed")]
    public int TotalCommentsProcessed { get; set; }
    
    [BsonElement("LastPageToken")]
    public string LastPageToken { get; set; }
    
    [BsonElement("IsFetching")]
    public bool IsFetching { get; set; }
}