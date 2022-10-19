using System.Text.Json.Serialization;

namespace TheNoobs.Serilog.Sinks.NewRelic.Internals;

internal class NewRelicLogCommon
{
    [JsonPropertyName("attributes")]
    public NewRelicAttributes Attributes { get; } = new();
}
