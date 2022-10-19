using System.Reflection;

namespace TheNoobs.Serilog.Sinks.NewRelic.Options;

public class NewRelicOptions
{
    internal NewRelicOptions(Assembly assembly)
    {
        ApplicationName = Environment.GetEnvironmentVariable("NEW_RELIC_APP_NAME") ?? assembly.GetName().Name!;
    }

    public string ApplicationName { get; set; }
    public string BaseUrl { get; set; } = "https://log-api.newrelic.com/log/v1";
    public string LicenseKey { get; set; } = Environment.GetEnvironmentVariable("NEW_RELIC_LICENSE_KEY") ?? string.Empty;
}
