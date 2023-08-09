using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbContracts;

public interface IFetchStatusModel
{
    [BsonId]
    string VideoId { get; }

    [BsonElement("TotalCommentsFetched")]
    int TotalCommentsFetched { get; }

    [BsonElement("IsFetching")]
    bool IsFetching { get; }
}