﻿
using Catalog.Query.Domain.Entities;
using Catalog.Query.Infrastructure.Persistence;
using Catalog.Query.Infrastructure.Repositories;
using MongoDB.Driver;

namespace Catalog.Query.Application.Repositories;

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
}