using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using TheNoobs.Serilog.Sinks.NewRelic.Options;

namespace TheNoobs.Serilog.Sinks.NewRelic.Extensions.DependencyInjection;

public static class Extensions
{
    public static void AddNewRelicLog(this IServiceCollection services,
        Assembly assembly,
        Action<NewRelicOptions, LoggerEnrichmentConfiguration>? builder = null)
    {
        var loggerConfiguration = new LoggerConfiguration();
        loggerConfiguration.WriteTo
            .NewRelic(assembly, options => builder?.Invoke(options, loggerConfiguration.Enrich))
            .CreateLogger();
        services.AddLogging(configure =>
        {
            configure.AddSerilog();
        });
    }

    public static void UseNewRelicLog(this IApplicationBuilder applicationBuilder)
    {
        var lifetime = applicationBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStopping.Register(Log.CloseAndFlush);
    }
}
