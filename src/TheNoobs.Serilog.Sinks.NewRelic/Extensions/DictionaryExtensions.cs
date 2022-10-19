namespace TheNoobs.Serilog.Sinks.NewRelic.Extensions;

public static class DictionaryExtensions
{
    internal static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        try
        {
            if (dictionary.ContainsKey(key))
            {
                return false;
            }
            dictionary.Add(key, value);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
