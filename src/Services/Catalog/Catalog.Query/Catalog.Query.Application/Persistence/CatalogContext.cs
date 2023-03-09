using Catalog.Query.Domain.Entities;
using Catalog.Query.Infrastructure.Persistence;
using Infrastructure.Common.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Query.Application.Persistence;


public class CatalogContext : ICatalogContext
{
    public CatalogContext(IOptions<MongoDbConfig> options)
    {
        MongoDbConfig config = options.Value;
        IMongoClient client = new MongoClient(config.ConnectionString);
        IMongoDatabase db = client.GetDatabase(config.Database);
        Products = db.GetCollection<Product>(config.Collection);
    }

    public IMongoCollection<Product> Products { get; }

}