using Refit;
using WeatherApp.Http.Attributes;
using WeatherApp.Models;

namespace WeatherApp.Http;

[Headers("Content-Type: application/json")]
public interface IGeocodingApi
{
    [Get("/search")]
    [ResponseCaching(60)]
    Task<SearchLocationResponse> SearchLocation(string name);
}
