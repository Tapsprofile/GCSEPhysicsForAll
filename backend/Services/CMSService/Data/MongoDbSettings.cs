namespace CMSService.Data;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "gcse_cms";
    public string ContentCollectionName { get; set; } = "content";
    public string ProgressCollectionName { get; set; } = "progress";
}
