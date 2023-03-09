using System.Threading.Tasks;
using Catalog.Query.Application.Persistence;
using Catalog.Query.Application.Repositories;
using Catalog.Query.Application.Tests.Fixtures;
using Catalog.Query.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Catalog.Query.Application.Tests;

public class ProductRepositoryTest : IClassFixture<ContextFixture>
{
    public ProductRepositoryTest(ContextFixture fixture)
    {
        Fixture = fixture;
    }

    public ContextFixture Fixture { get; }

    [Fact]
    public async Task Test_Retrieve_Inserted_Product_Via_Repository()
    {
        CatalogContext context = Fixture.CreateContext();
        Product product = new Product
        {
            Name = "Banana",
            Category = "Fruits",
            Description = "My very delicious fruit",
            Summary = "My very delicious fruit",
            ImageFile = "google.com",
            Price = 100.14m
        };
        await context.Products.InsertOneAsync(product);

        ProductRepository repository = new(context);
        var prod = await repository.GetProductAsync(product.Id!);
        prod.Should().NotBeNull();
    }
}