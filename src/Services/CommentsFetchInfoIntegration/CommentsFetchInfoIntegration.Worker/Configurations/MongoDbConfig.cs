namespace CommentsFetchInfoIntegration.Worker.Configurations;

public class MongoDbConfig
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string VideoFetchInfoCollectionName { get; set; }
}