using System;
using CacheManager.Core;

namespace SKBKontur.TaskManagerClient.Caching
{
    public class MemoryCache : IMemoryCache
    {
        private readonly ICacheManager<object> cacheImpl;
        private readonly TimeSpan dafaultTtl;

        public MemoryCache(ICacheManager<object> cacheImpl, TimeSpan dafaultTtl)
        {
            this.cacheImpl = cacheImpl;
            this.dafaultTtl = dafaultTtl;
        }

        public T GetOrLoad<T>(string key, Func<T> loader, TimeSpan? ttl = null)
        {
            var result = cacheImpl.Get<T>(key);
            if (result != null)
            {
                return result;
            }

            try
            {
                result = loader();
                cacheImpl.AddOrUpdate(key, result, value => result);
                cacheImpl.Expire(key, ttl ?? dafaultTtl);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Fail to load data for key {key}.", e);
            }
        }
    }
}