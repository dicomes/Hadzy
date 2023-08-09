using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbContracts;

public interface IFetchStatusModel
{
    [BsonId]
    string VideoId { get; }

    [BsonElement("TotalCommentsFetched")]
    int TotalCommentsFetched { get; }
    
    [BsonElement("LastPageToken")]
    public string LastPageToken { get; set; }

    [BsonElement("IsFetching")]
    bool IsFetching { get; }
}