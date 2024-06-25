using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ConfigManager.Core.Cache
{
    public class RedisService : ICacheService
    {
        private readonly IDatabase _db;

        public RedisService(string connectionString)
        {
            var redis = ConnectionMultiplexer.Connect(connectionString);
            _db = redis.GetDatabase();
        }

        public async Task AddAsync(string key, string value, TimeSpan expire)
        {
            await _db.StringSetAsync(key, value, expire);
        }

        public T GetFromJson<T>(string key)
        {
            var value = _db.StringGet(key);

            if (value.HasValue == false)
            {
                return default!;
            }

            return JsonConvert.DeserializeObject<T>(value);

            // return (T)Convert.ChangeType(value, typeof(T));
        }


        public async Task DeleteAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
    }
}