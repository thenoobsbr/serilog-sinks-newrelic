using System.Text;
using System.Text.Json;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using TheNoobs.Serilog.Sinks.NewRelic.Internals;
using TheNoobs.Serilog.Sinks.NewRelic.Options;

namespace TheNoobs.Serilog.Sinks.NewRelic;

public class NewRelicPeriodBatchSink : IBatchedLogEventSink
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };
    private readonly HttpClient _client;
    private readonly NewRelicOptions _options;

    public NewRelicPeriodBatchSink(NewRelicOptions options)
    {
        _client = new HttpClient();
        _options = options;
    }

    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
        {
            throw new ArgumentNullException(nameof(NewRelicOptions.BaseUrl));
        }

        if (string.IsNullOrWhiteSpace(_options.LicenseKey))
        {
            throw new ArgumentNullException(nameof(NewRelicOptions.LicenseKey));
        }

        var logEvents = batch.ToArray();
        var newRelicBatch = new NewRelicLogBatch(logEvents);
        newRelicBatch.Common.Attributes.ApplicationName = _options.ApplicationName;

        var json = JsonSerializer.Serialize(new[] { newRelicBatch }, _serializerOptions);
        
        using var request = new HttpRequestMessage(HttpMethod.Post, _options.BaseUrl);
        request.Headers.Add("Api-Key", _options.LicenseKey);

        const string JSON_TYPE = "application/json";
        
        request.Content =
            new StringContent(json, Encoding.UTF8, JSON_TYPE);
        var response = await _client.SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public Task OnEmptyBatchAsync()
    {
        // Do nothing
        return Task.CompletedTask;
    }
}
