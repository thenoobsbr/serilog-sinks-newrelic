using System.Text.Json.Serialization;

namespace TheNoobs.Serilog.Sinks.NewRelic.Internals;

internal readonly record struct NewRelicLogCommon()
{
    [JsonPropertyName("attributes")]
    public NewRelicAttributes Attributes { get; } = new();
}
