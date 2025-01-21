namespace TaskManager.Domain
{
    public interface IRedisRepository
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
        Task<bool> DeleteAsync(string key);
        Task<bool> KeyExistsAsync(string key);
    }
}
