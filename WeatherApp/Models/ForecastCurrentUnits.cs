using System.Text.Json.Serialization;

namespace WeatherApp.Models;

public partial class ForecastCurrentUnits
{
    [JsonPropertyName("interval")]
    public string Interval { get; set; } = default!;

    [JsonPropertyName("temperature_2m")]
    public string Temperature { get; set; } = default!;

    [JsonPropertyName("relative_humidity_2m")]
    public string RelativeHumidity { get; set; } = default!;

    [JsonPropertyName("wind_speed_10m")]
    public string WindSpeed { get; set; } = default!;
}