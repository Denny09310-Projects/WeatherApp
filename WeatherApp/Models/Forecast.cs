using System.Text.Json.Serialization;
using WeatherApp.Http.Converters;

namespace WeatherApp.Models;

public partial class Forecast
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationtimeMs { get; set; }

    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }

    [JsonPropertyName("current_units")]
    public CurrentUnits CurrentUnits { get; set; } = default!;

    [JsonPropertyName("current")]
    public Current Current { get; set; } = default!;
}

public partial class Current
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

public partial class CurrentUnits
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