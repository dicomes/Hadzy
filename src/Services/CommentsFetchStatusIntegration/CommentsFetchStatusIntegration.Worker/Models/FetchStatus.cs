using MongoDB.Bson.Serialization.Attributes;
using MongoDbContracts;

namespace CommentsFetchStatusIntegration.Worker.Models;

public class FetchStatus : IFetchStatusModel
{
    [BsonId]
    public string VideoId { get; set; }
    
    [BsonElement("TotalCommentsFetched")]
    public int TotalCommentsFetched { get; set; }
    
    [BsonElement("LastPageToken")]
    public string LastPageToken { get; set; }
    
    [BsonElement("IsFetching")]
    public bool IsFetching { get; set; }
}