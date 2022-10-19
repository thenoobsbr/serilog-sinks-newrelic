using System.Text.Json.Serialization;
using Serilog.Events;

namespace TheNoobs.Serilog.Sinks.NewRelic.Internals;

internal class NewRelicAttributes : Dictionary<string, object>
{
    public NewRelicAttributes()
    {
    }

    public NewRelicAttributes(LogEvent @event)
    {
        foreach (var property in @event.Properties)
        {
            TryAddOrUpdate(property.Key, property.Value);
        }
    }
    
    [JsonIgnore]
    public string ApplicationName
    {
        get => TryGetOrDefault(nameof(ApplicationName), string.Empty);
        set => TryAddOrUpdate(nameof(ApplicationName), value);
    }

    private T TryGetOrDefault<T>(string key, T defaultValue)
    {
        if (!TryGetValue(key, out var value)
            || Equals(value, default(T)))
        {
            return defaultValue;
        }

        return (T) value;
    }

    private void TryAddOrUpdate(string key, object value)
    {
        if (ContainsKey(key))
        {
            this[key] = value;
            return;
        }

        Add(key, value);
    }
}
