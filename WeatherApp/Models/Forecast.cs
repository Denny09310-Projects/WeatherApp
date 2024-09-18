using System.Text.Json.Serialization;

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
    public ForecastCurrentUnits CurrentUnits { get; set; } = default!;

    [JsonPropertyName("current")]
    public ForecastCurrent Current { get; set; } = default!;
}
