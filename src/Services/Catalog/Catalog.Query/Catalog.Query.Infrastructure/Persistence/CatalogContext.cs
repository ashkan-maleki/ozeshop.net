using Infrastructure.Common.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Query.Infrastructure.Persistence;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IOptions<MongoDbConfig> options)
    {
        MongoDbConfig config = options.Value;
        IMongoClient client = new MongoClient(config.ConnectionString);
        IMongoDatabase db = client.GetDatabase(config.Database);
        CreateModel(db);
    }

    private void CreateModel(IMongoDatabase db)
    {
        
    }
}