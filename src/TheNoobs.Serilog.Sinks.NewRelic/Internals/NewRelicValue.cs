using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog.Events;
#if NETSTANDARD
using TheNoobs.Serilog.Sinks.NewRelic.Extensions;
#endif

namespace TheNoobs.Serilog.Sinks.NewRelic.Internals;

[JsonConverter(typeof(Converter))]
internal readonly record struct NewRelicValue
{
    public NewRelicValue(object? value)
    {
        Value = value;
    }

    public object? Value { get; }

    public static NewRelicValue? Create(LogEventPropertyValue value)
    {
        return value switch
        {
            ScalarValue scalarValue => scalarValue.Value.GetType().IsPrimitive
                ? new NewRelicValue(scalarValue.Value)
                : new NewRelicValue(scalarValue.Value.ToString()!),
            DictionaryValue dictionaryValue => Create(dictionaryValue),
            SequenceValue sequenceValue => Create(sequenceValue),
            StructureValue structureValue => Create(structureValue),
            _ => null
        };
    }

    private static NewRelicValue Create(StructureValue structureValue)
    {
        var result = new Dictionary<object, object?>();
        result.TryAdd(nameof(structureValue.TypeTag), new NewRelicValue(structureValue.TypeTag ?? "AnonymousObject"));
        foreach (var property in structureValue.Properties)
        {
            var propKey = property.Name!;
            var propValue = Create(property.Value);
            result.TryAdd(propKey, propValue);
        }
        return new NewRelicValue(result);
    }

    private static NewRelicValue Create(DictionaryValue dictionaryValue)
    {
        var result = new Dictionary<object, object?>();
        foreach (var element in dictionaryValue.Elements)
        {
            var key = element.Key.Value.ToString()!;
            var value = Create(element.Value);
            result.TryAdd(key, value);
        }
        return new NewRelicValue(result);
    }
    
    private static NewRelicValue Create(SequenceValue sequenceValue)
    {
        var result = new NewRelicValue?[sequenceValue.Elements.Count];
        for (var i = 0; i < sequenceValue.Elements.Count; i++)
        {
            result[i] = Create(sequenceValue.Elements[i]);
        }
        return new NewRelicValue(result);
    }
    
    class Converter : JsonConverter<NewRelicValue>
    {
        public override NewRelicValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = JsonSerializer.Deserialize<object?>(ref reader);
            return new NewRelicValue(value);
        }

        public override void Write(Utf8JsonWriter writer, NewRelicValue value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}
