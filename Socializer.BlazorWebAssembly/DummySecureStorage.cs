using Common.Utils;
using System.Collections.Concurrent;

namespace Socializer.BlazorWebAssembly;

internal class DummySecureStorage : ISecureStorage
{
    // TODO: To be replaced by proper auth, storing tokens client side is temporary anyway
    private static readonly ConcurrentDictionary<string, string> storage = new();

    public Task<string?> GetAsync(string key)
    {
        storage.TryGetValue(key, out var val);
        return Task.FromResult(val);
    }

    public bool Remove(string key)
    {
        return storage.TryRemove(key, out _);
    }

    public void RemoveAll()
    {
        storage.Clear();
    }

    public Task SetAsync(string key, string value)
    {
        return new Task(() => storage.AddOrUpdate(key, value, (x, y) => value));
    }
}
