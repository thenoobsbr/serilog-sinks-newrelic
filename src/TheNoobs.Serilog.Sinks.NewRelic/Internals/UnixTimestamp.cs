using System.Text.Json;
using System.Text.Json.Serialization;

namespace TheNoobs.Serilog.Sinks.NewRelic.Internals;

[JsonConverter(typeof(Converter))]
internal readonly record struct UnixTimestamp
{
    private static readonly DateTimeOffset _epoch = new(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

    public UnixTimestamp(long value)
    {
        Value = value;
    }

    public long Value { get; }

    public static UnixTimestamp Create()
    {
        return Create(DateTimeOffset.UtcNow);
    }
    
    public static UnixTimestamp Create(DateTimeOffset date)
    {
        return new UnixTimestamp((long) date.Subtract(_epoch).TotalMilliseconds);
    }

    class Converter : JsonConverter<UnixTimestamp>
    {
        public override UnixTimestamp Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = JsonSerializer.Deserialize<long>(ref reader, options);
            return new UnixTimestamp(value);
        }

        public override void Write(Utf8JsonWriter writer, UnixTimestamp value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}
