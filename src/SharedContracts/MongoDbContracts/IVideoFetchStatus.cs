using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbContracts;

public interface IVideoFetchStatus
{
    [BsonId]
    string VideoId { get; }

    [BsonElement("TotalCommentsProcessed")]
    int TotalCommentsProcessed { get; }
    
    [BsonElement("LastPageToken")]
    public string LastPageToken { get; set; }

    [BsonElement("IsFetching")]
    bool IsFetching { get; }
}