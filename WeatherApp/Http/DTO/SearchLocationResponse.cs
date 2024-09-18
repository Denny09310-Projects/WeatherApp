#nullable enable
#pragma warning disable CS8618
#pragma warning disable CS8601
#pragma warning disable CS8603

namespace WeatherApp.Models;
using System.Text.Json.Serialization;

public partial class SearchLocationResponse
{
    [JsonPropertyName("results")]
    public SearchLocationResult[] Results { get; set; }
}
#pragma warning restore CS8618
#pragma warning restore CS8601
#pragma warning restore CS8603
