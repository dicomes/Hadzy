namespace CommentsFetchStatusIntegration.Worker.Configurations.Interfaces;

public interface IMongoDbConfig
{
    public string ConnectionString { get; }
    public string DatabaseName { get; }
    public string CommentsFetchStatusCollectionName { get; }
}