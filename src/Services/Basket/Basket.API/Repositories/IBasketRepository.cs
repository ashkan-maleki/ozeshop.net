using System.Text.Json.Serialization;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasket(string userName);
        Task<ShoppingCart?> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userName);
    }

    class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ShoppingCart?> GetBasket(string userName)
        {
            var basket = await 
                _redisCache.GetStringAsync(userName);
            if (!String.IsNullOrEmpty(basket))
            {
                return JsonConvert.DeserializeObject<ShoppingCart>(basket);
            }

            return null;
        }

        public async Task<ShoppingCart?> 
            UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName,
                JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName ?? "");
        }

        public async Task DeleteBasket(string userName)
            => await _redisCache.RemoveAsync(userName);
    }
}
