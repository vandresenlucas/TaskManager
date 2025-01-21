using Microsoft.Extensions.Logging;
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
        private readonly ILogger<RedisRepository> _logger;

        public RedisRepository(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisRepository> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
            _expirationMinutes = TimeSpan.FromMinutes(RedisOptions.DefaultExpirationMinutes);
            _logger = logger;
        }

        public async Task<bool> DeleteAsync(string key)
        {
            _logger.LogInformation($"Limpando cache do Redis. Key: {key}");
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            _logger.LogInformation($"Buscando dados no cache do Redis. Key: {key}");
            var value = await _database.StringGetAsync(key);

            if (!value.HasValue)
                return default;

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> KeyExistsAsync(string key)
            => await _database.KeyExistsAsync(key); 

        public async Task SetAsync<T>(string key, T value)
        {
            _logger.LogInformation($"Atualizando dados do Redis. Key: {key}");

            var serializedValue = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key, serializedValue, _expirationMinutes);
        }
    }
}
