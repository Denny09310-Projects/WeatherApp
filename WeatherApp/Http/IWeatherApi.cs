using Refit;
using WeatherApp.Http.Attributes;
using WeatherApp.Models;

namespace WeatherApp.Http;

[Headers("Content-Type: application/json")]
public interface IWeatherApi
{
    [Get("/forecast")]
    [ResponseCaching(3600)]
    Task<ApiResponse<Forecast>> GetForecastAsync(double latitude, double longitude, string[] current);
}
