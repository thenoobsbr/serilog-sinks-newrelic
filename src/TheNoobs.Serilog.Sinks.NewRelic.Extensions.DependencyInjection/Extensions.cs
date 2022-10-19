using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Sinks.PeriodicBatching;
using TheNoobs.Serilog.Sinks.NewRelic.Options;

namespace TheNoobs.Serilog.Sinks.NewRelic.Extensions.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddNewRelicLog(this IServiceCollection services,
        Assembly mainAssembly,
        Action<NewRelicOptions, LoggerEnrichmentConfiguration>? builder = null,
        LoggerConfiguration? loggerConfiguration = null)
    {
        loggerConfiguration ??= new LoggerConfiguration();
        services.AddSingleton<IBatchedLogEventSink, NewRelicPeriodBatchSink>();
        Log.Logger = loggerConfiguration.WriteTo
            .NewRelic(mainAssembly, options => builder?.Invoke(options, loggerConfiguration.Enrich))
            .CreateLogger();
        services.AddLogging(configure =>
        {
            configure.AddSerilog();
        });
        return services;
    }

    public static IApplicationBuilder UseNewRelicLog(this IApplicationBuilder applicationBuilder)
    {
        var lifetime = applicationBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStopping.Register(Log.CloseAndFlush);
        return applicationBuilder;
    }
}
