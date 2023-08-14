using MongoDB.Bson.Serialization.Attributes;
using MongoDbContracts;

namespace CommentsFetchStatus.MinimalApi.Models;

public class VideoFetchInfo : IVideoFetchInfo
{
    [BsonId]
    public string VideoId { get; set; }
    
    [BsonElement("CommentsCount")]
    public int CommentsCount { get; set; }
    
    [BsonElement("LastPageToken")]
    public string LastPageToken { get; set; }

    [BsonElement("Status")]
    public string Status { get; set; }
}