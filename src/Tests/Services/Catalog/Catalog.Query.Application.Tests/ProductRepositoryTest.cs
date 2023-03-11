using System.Collections.Generic;
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
        Product prod = await repository.GetProductAsync(product.Id!);
        prod.Should().NotBeNull();
    }
    
    
    [Fact]
    public async Task Test_Retrieve_Inserted_Product_Via_Repository_By_Name()
    {
        CatalogContext context = Fixture.CreateContext();
        ProductRepository repository = new(context);
        
        string name = "Banana";
        Product product = new Product
        {
            Name = name,
            Category = "Fruits",
            Description = "My very delicious fruit",
            Summary = "My very delicious fruit",
            ImageFile = "google.com",
            Price = 100.14m
        };
        await context.Products.InsertOneAsync(product);
        IEnumerable<Product> prod = await repository.GetProductByNameAsync(name);
        prod.Should().NotBeNull();
        prod.Should().NotBeEmpty();
        prod.Should().Contain(p => p.Id == product.Id);
    }
}