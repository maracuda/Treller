using System;

namespace SKBKontur.TaskManagerClient.Caching
{
    public interface IMemoryCache
    {
        T GetOrLoad<T>(string key, Func<T> loader, TimeSpan? ttl = null);
    }
}