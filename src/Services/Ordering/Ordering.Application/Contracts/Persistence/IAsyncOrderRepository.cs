using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IAsyncOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName);
    }
}