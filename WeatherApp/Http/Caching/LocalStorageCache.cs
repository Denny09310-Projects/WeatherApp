using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace WeatherApp.Http.Caching;

public class LocalStorageCache(ILocalStorageService storage, IOptions<MemoryCacheOptions> options) : ILocalStorageCache
{
    private readonly ILocalStorageService _storage = storage;
    private readonly MemoryCache _cache = new(options);

    ICacheEntry IMemoryCache.CreateEntry(object key)
    {
        var cacheEntry = _cache.CreateEntry(key);

        cacheEntry.RegisterPostEvictionCallback((evictedKey, value, reason, state) =>
        {
            _storage.RemoveItem(GenerateKey(evictedKey));
        });

        return cacheEntry;
    }

    public TItem Set<TItem>(object key, TItem value, MemoryCacheEntryOptions? options)
    {
        using ICacheEntry entry = _cache.CreateEntry(key);
        if (options != null)
        {
            entry.SetOptions(options);
        }

        entry.Value = value;
        _storage.SetItem(GenerateKey(key), value);

        return value;
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cache.Dispose();
        }
    }

    void IMemoryCache.Remove(object key)
    {
        _cache.Remove(key);
        _storage.RemoveItem(GenerateKey(key));
    }


    bool IMemoryCache.TryGetValue(object key, out object? value)
    {
        // Try getting value from memory cache first
        if (_cache.TryGetValue(key, out value))
        {
            return true;
        }

        // If not found in memory, check local storage
        var localStorageValue = _storage.GetItem<object>(GenerateKey(key));
        if (localStorageValue != null)
        {
            // If found in local storage, add it to memory cache and return
            var cacheEntry = _cache.CreateEntry(key);
            cacheEntry.Value = localStorageValue;
            cacheEntry.Dispose();
            value = localStorageValue;
            return true;
        }

        // Not found in both memory cache and local storage
        value = null;
        return false;
    }

    private static string GenerateKey(object key)
    {
        // Create a string key from object properties (this is a simple example)
        var keyProperties = JsonSerializer.Serialize(key);

        // Optionally, hash the serialized key if you want a shorter identifier
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(keyProperties));
        return Convert.ToBase64String(hash);
    }

}

public interface ILocalStorageCache : IMemoryCache
{
    TItem Set<TItem>(object key, TItem value, MemoryCacheEntryOptions? options);
}
