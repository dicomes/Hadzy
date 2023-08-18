using MongoDB.Bson.Serialization.Attributes;
using MongoDbContracts;

namespace CommentsFetchInfoManager.MinimalApi.Models;

public class VideoFetchInfo : IVideoFetchInfo
{
    [BsonId]
    public string VideoId { get; set; }
    
    [BsonElement("CommentsCount")]
    public int CommentsCount { get; set; }
    
    [BsonElement("CommentIds")]
    public List<string> CommentIds { get; set; }
    
    [BsonElement("LastPageToken")]
    public string LastPageToken { get; set; }

    [BsonElement("Status")]
    public string Status { get; set; }
}