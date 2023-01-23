using Catalog.Query.Domain.Entities;
using MongoDB.Driver;

namespace Catalog.Query.Infrastructure.Persistence;

public interface ICatalogContext
{
    public IMongoCollection<Product> Products { get; }
}