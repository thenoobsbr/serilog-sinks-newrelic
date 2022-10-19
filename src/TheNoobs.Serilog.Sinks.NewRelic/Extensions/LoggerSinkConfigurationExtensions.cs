using System.Reflection;
using Serilog;
using Serilog.Configuration;
using Serilog.Sinks.PeriodicBatching;
using TheNoobs.Serilog.Sinks.NewRelic.Options;

namespace TheNoobs.Serilog.Sinks.NewRelic.Extensions;

public static class LoggerSinkConfigurationExtensions
{
    public static LoggerConfiguration NewRelic(this LoggerSinkConfiguration loggerSinkConfiguration,
        Assembly assembly,
        Action<NewRelicOptions>? builder = null)
    {
        var options = new NewRelicOptions(assembly);
        builder?.Invoke(options);

        var sink = new NewRelicSink(options);
        
        return loggerSinkConfiguration.Sink(sink);
    }

    class NewRelicSink : PeriodicBatchingSink
    {
        public NewRelicSink(NewRelicOptions options) : base(new NewRelicPeriodBatchSink(options), new PeriodicBatchingSinkOptions())
        {
        }
    }
}
