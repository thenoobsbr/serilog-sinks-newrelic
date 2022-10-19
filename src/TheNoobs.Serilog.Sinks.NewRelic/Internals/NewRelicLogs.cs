using System.Collections;
using Serilog.Events;

namespace TheNoobs.Serilog.Sinks.NewRelic.Internals;

internal sealed class NewRelicLogs : IEnumerator<NewRelicLog>, IEnumerable<NewRelicLog>
{
    private readonly SemaphoreSlim _semaphore = new(0, 1);
    private readonly LogEvent[] _events;
    private int _index;
    private bool _disposed;
    public NewRelicLogs(LogEvent[] events)
    {
        _index = -1;
        _events = events;
    }
    
    public bool MoveNext()
    {
        try
        {
            _semaphore.Wait(TimeSpan.FromSeconds(1));

            if (_index >= _events.Length - 1)
            {
                return false;
            }

            Current = new NewRelicLog(_events[++_index]);
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            _semaphore.Release(1);
        }
    }

    public void Reset()
    {
        _index = -1;
    }

    public NewRelicLog Current { get; private set; }

    object IEnumerator.Current => Current;

    public IEnumerator<NewRelicLog> GetEnumerator()
    {
        return this;
    }

    ~NewRelicLogs()
    {
        Dispose(false);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        
        if (disposing)
        {
            // Do nothing
        }

        _disposed = true;
    }
}
