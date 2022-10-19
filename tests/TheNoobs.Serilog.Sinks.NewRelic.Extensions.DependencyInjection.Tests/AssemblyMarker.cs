using System.Reflection;

namespace TheNoobs.Serilog.Sinks.NewRelic.Extensions.DependencyInjection.Tests;

public class AssemblyMarker
{
    public static Assembly Assembly => typeof(AssemblyMarker).Assembly;
}
