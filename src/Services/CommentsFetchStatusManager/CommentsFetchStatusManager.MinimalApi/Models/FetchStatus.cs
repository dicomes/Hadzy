using MongoDB.Bson.Serialization.Attributes;
using MongoDbContracts;

namespace CommentsFetchStatus.MinimalApi.Models;

public class FetchStatus : IVideoFetchStatus
{
    [BsonId]
    public string VideoId { get; set; }

    [BsonElement("TotalCommentsProcessed")]
    public int TotalCommentsProcessed { get; }
    
    [BsonElement("LastPageToken")]
    public string LastPageToken { get; set; }
    
    [BsonElement("IsFetching")]
    public bool IsFetching { get; set; }
}