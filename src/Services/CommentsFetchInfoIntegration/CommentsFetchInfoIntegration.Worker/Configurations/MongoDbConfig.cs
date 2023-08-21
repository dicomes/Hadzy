using CommentsFetchInfoIntegration.Worker.Configurations.Interfaces;

namespace CommentsFetchInfoIntegration.Worker.Configurations;

public class MongoDbConfig : IMongoDbConfig
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string VideoFetchInfoCollectionName { get; set; }
}