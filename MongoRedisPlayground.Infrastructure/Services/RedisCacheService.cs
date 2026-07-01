using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MongoRedisPlayground.Domain.Services;

namespace MongoRedisPlayground.Infrastructure.Services;

public sealed class RedisCacheService(IDistributedCache cache) : ICacheService
{
    private readonly IDistributedCache _cache = cache;

    public async Task<T?> GetAsync<T>(string key)
    {
        string? json = await _cache.GetStringAsync(key);

        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        string json = JsonSerializer.Serialize(value);

        await _cache.SetStringAsync(key, json, options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}