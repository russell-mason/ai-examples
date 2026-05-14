namespace MicrosoftAgentFramework.Examples.Foundation.Services;

// For demonstration purposes only, this is a simple in-memory data store that allows you to add,
// retrieve, and remove items using string keys. It is not thread-safe and allows direct mutation
// of the stored items.
public class DataStore<T>
{
    private readonly Dictionary<string, T> _data = new();

    public int Count => _data.Count;

    public T Get(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        return _data.TryGetValue(key, out var value) 
            ? value 
            : throw new KeyNotFoundException($"An item with the key '{key}' was not found.");
    }

    public bool TryGet(string key, out T? item)
    {
        ArgumentNullException.ThrowIfNull(key);

        return _data.TryGetValue(key, out item);
    }

    public bool ContainsKey(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        return _data.ContainsKey(key);
    }

    public void Add(string key, T item)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (!_data.TryAdd(key, item))
        {
            throw new ArgumentException($"An item with the key '{key}' already exists.", nameof(key));
        }
    }

    public bool Remove(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        return _data.Remove(key);
    }

    public void Clear()
    {
        _data.Clear();
    }
}
