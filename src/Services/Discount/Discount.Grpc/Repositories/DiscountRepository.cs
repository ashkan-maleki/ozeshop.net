using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories;

class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private NpgsqlConnection GetConnection()
        => new NpgsqlConnection(
            _configuration
            .GetValue<string>("DatabaseSettings:ConnectionString"
            ));

    public async Task<Coupon> GetDiscountAsync(string productName)
    {
        await using var connection = GetConnection();
        var coupon = await connection
            .QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon " +
                "WHERE ProductName = @ProductName",
                new {ProductName = productName}
            );

        return coupon ?? new Coupon()
        {
            ProductName = "No Discount",
            Amount = 0,
            Description = "No Discount Desc"
        };
    }

    public async Task<bool> CreateDiscountAsync(Coupon coupon)
    {
        await using var connection = GetConnection();
        return await connection.ExecuteAsync(
            "INSERT INTO Coupon (ProductName, Description," +
            " Amount) VALUES (@ProductName, @Description," +
            " @Amount",
            new
            {
                ProductName = coupon.ProductName,
                Amount = coupon.Amount,
                Description = coupon.Description
            }
        ) != 0;
    }

    public async Task<bool> UpdateDiscountAsync(Coupon coupon)
    {
        await using var connection = GetConnection();
        return await connection.ExecuteAsync(
            "UPDATE Coupon SET ProductName = @ProductName," +
            " Description = @Description," +
            " Amount = @Amount",
            new
            {
                ProductName = coupon.ProductName,
                Amount = coupon.Amount,
                Description = coupon.Description
            }
        ) != 0;
    }

    public async Task<bool> DeleteDiscountAsync(string productName)
    {
        await using var connection = GetConnection();
        return await connection.ExecuteAsync(
            "DELETE FROM Coupon WHERE ProductName = @ProductName",
            new
            {
                ProductName = productName
            }
        ) != 0;
    }
}