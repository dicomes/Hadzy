using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbContracts;

public interface IVideoFetchInfo
{
    [BsonId]
    string VideoId { get; }

    [BsonElement("CommentsCount")]
    ulong CommentsCount { get; }
    
    [BsonElement("CommentIds")]
    List<string> CommentIds { get; }
    
    [BsonElement("LastPageToken")]
    string? LastPageToken { get;}

    [BsonElement("Status")]
    string? Status { get; }
    
    [BsonElement("CompletedTillFirstComment")]
    bool CompletedTillFirstComment { get; }
}