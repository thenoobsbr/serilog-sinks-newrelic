using System.Text.Json.Serialization;
using Serilog.Events;

namespace TheNoobs.Serilog.Sinks.NewRelic.Internals;

internal class NewRelicLog
{
    private readonly LogEvent _event;

    public NewRelicLog(LogEvent @event)
    {
        _event = @event;
    }

    [JsonPropertyName("timestamp")]
    public UnixTimestamp Timestamp => UnixTimestamp.Create(_event.Timestamp);
    
    [JsonPropertyName("attributes")]
    public NewRelicAttributes Attributes => new(_event);

    [JsonPropertyName("message")]
    public string Message => _event.RenderMessage();
}
