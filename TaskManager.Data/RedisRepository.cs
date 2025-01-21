using Newtonsoft.Json;
using StackExchange.Redis;
using TaskManager.CrossCutting.Configurations.Options;
using TaskManager.Domain;

namespace TaskManager.Data
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly TimeSpan _expirationMinutes;

        public RedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
            _expirationMinutes = TimeSpan.FromMinutes(RedisOptions.DefaultExpirationMinutes);
        }

        public async Task<bool> DeleteAsync(string key)
            => await _database.KeyDeleteAsync(key);

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);

            if (!value.HasValue)
                return default;

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> KeyExistsAsync(string key)
            => await _database.KeyExistsAsync(key); 

        public async Task SetAsync<T>(string key, T value)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key, serializedValue, _expirationMinutes);
        }
    }
}
