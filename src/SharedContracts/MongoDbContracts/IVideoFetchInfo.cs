using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbContracts;

public interface IVideoFetchInfo
{
    [BsonId]
    string VideoId { get; }

    [BsonElement("CommentsCount")]
    int CommentsCount { get; }
    
    [BsonElement("CommentIds")]
    List<string> CommentIds { get; }
    
    [BsonElement("LastPageToken")]
    string LastPageToken { get; set; }

    [BsonElement("Status")]
    string Status { get; }
}