using System;

namespace SKBKontur.TaskManagerClient.Caching
{
    public interface ICacheFactory
    {
        IMemoryCache CreateMemoryCache(string name, TimeSpan ttl);
    }
}