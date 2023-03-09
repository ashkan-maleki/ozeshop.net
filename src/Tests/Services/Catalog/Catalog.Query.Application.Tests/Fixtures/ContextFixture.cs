using Catalog.Query.Application.Persistence;
using Infrastructure.Common.Configuration;
using Microsoft.Extensions.Options;

namespace Catalog.Query.Application.Tests.Fixtures;

public class ContextFixture
{
    public CatalogContext CreateContext()
    {
        var op = Options.Create(new MongoDbConfig
        {
            Collection = "Products",
            Database = "ProductDb",
            ConnectionString = "mongodb://localhost:27017"
        });
        CatalogContext dbContext = new(op);

        return dbContext;
    }
}