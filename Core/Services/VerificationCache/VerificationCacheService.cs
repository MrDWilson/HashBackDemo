using Microsoft.Extensions.Caching.Memory;

namespace HashBack.Services;

public class VerificationCacheService : IVerificationCacheService
{
    private readonly MemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public VerificationCacheService()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
        _cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        };
    }

    public void Add(string key, string value)
    {
        _cache.Set(key, value, _cacheOptions);
    }

    public bool Exists(string key)
    {
        return _cache.TryGetValue(key, out _);
    }

    public string? GetAndRemove(string key)
    {
        if(_cache.TryGetValue(key, out string? value))
        {
            _cache.Remove(key);
            return value;
        }
        else return null;
    }
}