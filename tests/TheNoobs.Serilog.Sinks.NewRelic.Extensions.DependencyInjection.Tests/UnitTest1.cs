using Serilog;

namespace TheNoobs.Serilog.Sinks.NewRelic.Extensions.DependencyInjection.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.NewRelic(AssemblyMarker.Assembly, 
                options =>
                {
                    options.LicenseKey = "";
                })
            .CreateLogger();
        
        Log.Information("[{ApplicationName}] {Request}",
            "TheNoobs.Tests",
            new
            {
                Message = "Test Message"
            });
        
        Log.CloseAndFlush();
    }
}
