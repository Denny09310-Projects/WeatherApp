#nullable enable
#pragma warning disable CS8618

using WeatherApp;

namespace WeatherApp.Http.Converters;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateOnlyConverter(string? serializationFormat) : JsonConverter<DateOnly>
{
    private readonly string serializationFormat = serializationFormat ?? "yyyy-MM-dd";
    public DateOnlyConverter() : this(null) { }

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return DateOnly.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(serializationFormat));
}

#pragma warning restore CS8618
