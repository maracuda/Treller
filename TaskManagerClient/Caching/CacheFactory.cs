using System;

namespace SKBKontur.TaskManagerClient.Caching
{
    public class CacheFactory : ICacheFactory
    {

        public IMemoryCache CreateMemoryCache(string name, TimeSpan ttl)
        {
            return new MemoryCache(CacheManager.Core.CacheFactory.Build(name, s => s.WithDictionaryHandle()), ttl);
        }
    }
}