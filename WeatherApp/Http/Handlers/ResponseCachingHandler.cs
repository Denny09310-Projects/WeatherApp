using Microsoft.Extensions.Caching.Memory;
using Refit;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using System.Text.Json;
using WeatherApp.Http.Attributes;

namespace WeatherApp.Http.Handlers;

internal class ResponseCachingHandler(ILocalStorageCache cache) : DelegatingHandler
{
    private readonly ILocalStorageCache _cache = cache;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string key = request.RequestUri!.AbsoluteUri;

        if (request.Method != HttpMethod.Post && _cache.TryGetValue(key, out var value) && value is JsonElement json)
        {
            return DeserializeResponse(json);
        }

        if (!TryGetResponseCaching(request, out var expirationTime))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        return await SendAndCacheAsync(request, key, expirationTime, cancellationToken);
    }

    private static HttpResponseMessage DeserializeResponse(JsonElement json)
    {
        var cached = json.Deserialize<CachedResponse>();
        ArgumentNullException.ThrowIfNull(cached);

        return new HttpResponseMessage(cached.StatusCode)
        {
            Content = new StringContent(cached.Content, Encoding.UTF8, "application/json")
        };
    }

    private async Task<HttpResponseMessage> SendAndCacheAsync(HttpRequestMessage request, string key, TimeSpan? expirationTime, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        _cache.Set(key, new CachedResponse(content, response.StatusCode), new MemoryCacheEntryOptions { SlidingExpiration = expirationTime });

        return response;
    }

    private static bool TryGetResponseCaching(HttpRequestMessage request, [MaybeNullWhen(false)] out TimeSpan? expirationTime)
    {
        expirationTime = default;

        var key = new HttpRequestOptionsKey<RestMethodInfo>(HttpRequestMessageOptions.RestMethodInfo);
        if (!request.Options.TryGetValue(key, out var option) || option is not RestMethodInfo method)
        {
            return false;
        }

        if (method.MethodInfo.GetCustomAttribute<ResponseCachingAttribute>() is not ResponseCachingAttribute attribute)
        {
            return false;
        }

        expirationTime = attribute.ExpirationTime;
        return true;
    }
}
