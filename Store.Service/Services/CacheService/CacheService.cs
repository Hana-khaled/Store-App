using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Service.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            // injecting IConectionMultiplexer
            // and getting in memory database

            _database = redis.GetDatabase();
        }
        public async Task<string> GetCacheResponseAsync(string key)
        {
            var cachedResponse = await _database.StringGetAsync(key);

            if (cachedResponse.IsNullOrEmpty)
                return null;

            return cachedResponse.ToString();
        }

        public async Task SetCacheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response == null)
                return; 

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonResponse = JsonSerializer.Serialize(response, options);

           await _database.StringSetAsync(key, jsonResponse, timeToLive);
        }
    }
}
