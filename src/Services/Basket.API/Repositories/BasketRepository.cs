using System.Text.Json;
using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);//removing the user from redis cache
        }
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                throw new Exception("Invalid Username");
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);//converting json to object
        }
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName,JsonConvert.SerializeObject(basket));//convert object into json format
            return await GetBasket(basket.UserName);

        }
    }
}
