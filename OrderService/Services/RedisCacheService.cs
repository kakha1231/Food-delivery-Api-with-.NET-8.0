using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace OrderService.Services;

public class RedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> Get<T>(string key)
    {
        var value = await _cache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        
        return default;
    }

    public async Task Set<T>(string key, T value)
    { 
        await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });
    }
    
    public async Task<bool> Remove(string key)
    { 
        var value = await _cache.GetStringAsync(key);

        if (string.IsNullOrEmpty(value))
        {
            return false;
        }
        await _cache.RemoveAsync(key);

        return true;
    }

    public async Task Update<T>(string key, T value)
    {
        await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });
    }
    
}