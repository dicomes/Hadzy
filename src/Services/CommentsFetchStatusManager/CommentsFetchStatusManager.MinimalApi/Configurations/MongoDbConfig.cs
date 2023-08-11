namespace CommentsFetchStatus.MinimalApi.Configurations;

public class MongoDbConfig
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CommentsFetchStatusCollectionName { get; set; }
}