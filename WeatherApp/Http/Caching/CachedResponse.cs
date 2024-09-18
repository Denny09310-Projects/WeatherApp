using System.Net;
using System.Text.Json.Serialization;

namespace WeatherApp.Http.Caching;

public record CachedResponse(
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("statusCode")][property: JsonConverter(typeof(JsonStringEnumConverter))] HttpStatusCode StatusCode);
