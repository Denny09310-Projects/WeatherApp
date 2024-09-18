using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeatherApp.Http.Caching;

namespace WeatherApp.Http;

internal class ResponseCachingHandler(ILocalStorageCache cache) : DelegatingHandler
{
    private static readonly MemoryCacheEntryOptions _options = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(5),
    };

    private readonly ILocalStorageCache _cache = cache;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string key = request.RequestUri!.AbsoluteUri;

        if (request.Method != HttpMethod.Post && _cache.TryGetValue(key, out var value) && value is JsonElement json)
        {
            var cached = json.Deserialize<CachedResponse>();
            ArgumentNullException.ThrowIfNull(cached);

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

public record CachedResponse(
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("statusCode")][property: JsonConverter(typeof(JsonStringEnumConverter))] HttpStatusCode StatusCode);
