using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbContracts;

public interface IVideoCommentsStatusModel
{
    [BsonId]
    string VideoId { get; }

    [BsonElement("TotalCommentsFetched")]
    int TotalCommentsFetched { get; }

    [BsonElement("IsFetching")]
    bool IsFetching { get; }
}