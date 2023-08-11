using CommentsFetchStatusIntegration.Worker.Configurations.Interfaces;

namespace CommentsFetchStatusIntegration.Worker.Configurations;

public class MongoDbConfig : IMongoDbConfig
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CommentsFetchStatusCollectionName { get; set; }
}