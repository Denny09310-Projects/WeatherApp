using System.Text.Json.Serialization;
using WeatherApp.Http.Converters;

namespace WeatherApp.Models;

public partial class ForecastCurrent
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("interval")]
    public double Interval { get; set; }

    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }

    [JsonPropertyName("relative_humidity_2m")]
    public double RelativeHumidity { get; set; }

    [JsonPropertyName("is_day"), JsonConverter(typeof(BoolJsonConverter))]
    public bool IsDay { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed { get; set; }

    [JsonPropertyName("weather_code")]
    public int WeatherCode { get; set; }
}
