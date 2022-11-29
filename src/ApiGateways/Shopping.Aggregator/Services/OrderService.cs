using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

class OrderService : IOrderService
{
    public Task<IEnumerable<OrderResponseModel>> GetOrderByUserName(string userName)
    {
        throw new NotImplementedException();
    }
}