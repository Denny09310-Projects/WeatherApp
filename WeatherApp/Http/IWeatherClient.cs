using Refit;
using WeatherApp.Models;

namespace WeatherApp.Http;

[Headers("Content-Type: application/json")]
public interface IWeatherClient
{
    [Get("/forecast")]
    Task<ApiResponse<Forecast>> GetForecastAsync(double latitude, double longitude, string[] current);
}
