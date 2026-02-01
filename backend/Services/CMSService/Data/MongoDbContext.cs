using MongoDB.Driver;
using Shared.Models;

namespace CMSService.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<Content> Content =>
        _database.GetCollection<Content>("content");

    public IMongoCollection<ContentProgress> ContentProgress =>
        _database.GetCollection<ContentProgress>("progress");
}
