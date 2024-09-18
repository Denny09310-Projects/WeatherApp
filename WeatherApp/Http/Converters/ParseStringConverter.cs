#nullable enable
#pragma warning disable CS8618

namespace WeatherApp.Http.Converters;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class ParseStringConverter : JsonConverter<long>
{
    public override bool CanConvert(Type t) => t == typeof(long);

    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (long.TryParse(value, out long l))
        {
            return l;
        }
        throw new Exception("Cannot unmarshal type long");
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.ToString(), options);
    }

    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
}

#pragma warning restore CS8618
