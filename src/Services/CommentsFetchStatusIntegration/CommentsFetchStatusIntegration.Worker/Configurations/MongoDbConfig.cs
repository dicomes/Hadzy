namespace CommentsFetchStatusIntegration.Worker.Configurations;

public class MongoDbConfig
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CommentsStatusCollectionName { get; set; }
}