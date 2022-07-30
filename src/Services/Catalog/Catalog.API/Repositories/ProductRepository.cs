using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _catalogContext;

    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync() =>
        await _catalogContext
            .Products
            .Find(p => true)
            .ToListAsync();

    public async Task<Product> GetProductAsync(string id) =>
        await _catalogContext
            .Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Product>> 
        GetProductByNameAsync(string name) =>
        await _catalogContext
            .Products
            .Find(Builders<Product>.Filter
                .Eq(p => p.Name, name))
            .ToListAsync();

    public async Task<IEnumerable<Product>> 
        GetProductByCategoryAsync(string categoryName) =>
        await _catalogContext
            .Products
            .Find(Builders<Product>.Filter
                .Eq(p => p.Category, categoryName))
            .ToListAsync();

    public async Task CreateProductAsync(Product product) 
        => await _catalogContext.Products
            .InsertOneAsync(product);

    public async Task<bool> UpdateProductAsync(Product product)
    {
        ReplaceOneResult updateResult = await _catalogContext
            .Products
            .ReplaceOneAsync(filter: g => g.Id == product.Id,
                replacement: product);

        return updateResult.IsAcknowledged 
            && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        FilterDefinition<Product> filter =
            Builders<Product>.Filter.Eq(p => p.Id, id);

        DeleteResult deleteResult = await _catalogContext
            .Products
            .DeleteOneAsync(filter);

        return deleteResult.IsAcknowledged 
            && deleteResult.DeletedCount > 0;
    }
}