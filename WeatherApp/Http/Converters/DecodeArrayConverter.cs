#nullable enable
#pragma warning disable CS8618

namespace WeatherApp.Http.Converters;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class DecodeArrayConverter : JsonConverter<long[]>
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(long[]);

    public override long[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read();
        var value = new List<long>();
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            var converter = ParseStringConverter.Singleton;
            var arrayItem = converter.Read(ref reader, typeof(long), options);
            value.Add(arrayItem);
            reader.Read();
        }
        return [.. value];
    }

    public override void Write(Utf8JsonWriter writer, long[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var arrayItem in value)
        {
            var converter = ParseStringConverter.Singleton;
            converter.Write(writer, arrayItem, options);
        }
        writer.WriteEndArray();
    }

    public static readonly DecodeArrayConverter Singleton = new();
}

#pragma warning restore CS8618
