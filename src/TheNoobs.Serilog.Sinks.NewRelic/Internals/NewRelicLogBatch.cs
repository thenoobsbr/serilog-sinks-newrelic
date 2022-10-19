using System.Text.Json.Serialization;
using Serilog.Events;

namespace TheNoobs.Serilog.Sinks.NewRelic.Internals;

internal readonly record struct NewRelicLogBatch
{
    public NewRelicLogBatch(LogEvent[] events)
    {
        Common = new NewRelicLogCommon();
        Logs = new NewRelicLogs(events);
    }

    [JsonPropertyName("common")]
    public NewRelicLogCommon Common { get; }
    
    [JsonPropertyName("logs")]
    public NewRelicLogs Logs { get; }
}
