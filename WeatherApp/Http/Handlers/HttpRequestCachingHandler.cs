using System.Net;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace WeatherApp.Http;

internal class ResponseCachingHandler(IMemoryCache cache) : DelegatingHandler
{
    private static readonly MemoryCacheEntryOptions _options = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(5),
    };

    private readonly IMemoryCache _cache = cache;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string key = request.RequestUri!.AbsoluteUri;

        if (request.Method != HttpMethod.Post && _cache.TryGetValue(key, out var value) && value is CachedResponse cached)
        {
            return new HttpResponseMessage(cached.StatusCode)
            {
                Content = new StringContent(cached.Content, Encoding.UTF8, "application/json")
            };
        }

        var response = await base.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        _cache.Set(key, new CachedResponse(content, response.StatusCode), _options);

        return response;
    }
}

public record CachedResponse(string Content, HttpStatusCode StatusCode);
