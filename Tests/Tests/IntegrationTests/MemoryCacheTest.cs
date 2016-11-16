using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SKBKontur.TaskManagerClient.Caching;
using Assert = SKBKontur.Treller.Tests.UnitWrappers.Assert;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests
{
    public class MemoryCacheTest : IntegrationTest
    {
        private const string testKey = "testKey";
        private IMemoryCache memoryCache;

        public MemoryCacheTest() : base()
        {
            memoryCache = container.Get<ICacheFactory>().CreateMemoryCache("test", TimeSpan.FromMilliseconds(500));
        }

        [Fact]
        public void TestGetFromCacheValueType()
        {
            var value = 0;
            var loader = new Func<int>(() => ++value);

            Assert.AreEqual(1, memoryCache.GetOrLoad(testKey, loader));
            Assert.AreEqual(1, memoryCache.GetOrLoad(testKey, loader));
            Assert.AreEqual(1, memoryCache.GetOrLoad(testKey, loader));
        }

        [Fact]
        public void TestGetFromCacheRefType()
        {
            var value = 0;
            var loader = new Func<string>(() => (++value).ToString());

            Assert.AreEqual("1", memoryCache.GetOrLoad(testKey, loader));
            Assert.AreEqual("1", memoryCache.GetOrLoad(testKey, loader));
            Assert.AreEqual("1", memoryCache.GetOrLoad(testKey, loader));
        }

        [Fact]
        public void TestGetFromCacheWithGlobalTtl()
        {
            var value = 0;
            var loader = new Func<int>(() => ++value);

            Assert.AreEqual(1, memoryCache.GetOrLoad(testKey, loader));
            Thread.Sleep(00);
            Assert.AreEqual(2, memoryCache.GetOrLoad(testKey, loader));
        }

        [Fact]
        public void TestGetFromCacheWithLocalTtl()
        {
            var value = 0;
            var loader = new Func<int>(() => ++value);

            Assert.AreEqual(1, memoryCache.GetOrLoad(testKey, loader, TimeSpan.FromMilliseconds(100)));
            Thread.Sleep(100);
            Assert.AreEqual(2, memoryCache.GetOrLoad(testKey, loader, TimeSpan.FromMilliseconds(100)));
            Thread.Sleep(100);
            Assert.AreEqual(3, memoryCache.GetOrLoad(testKey, loader, TimeSpan.FromMilliseconds(100)));
        }

        [Fact]
        public void TestConcurrentGet()
        {
            var value = 0;
            var loader = new Func<int>(() => ++value);

            Parallel.For(1, 100, number =>
            {
                memoryCache.GetOrLoad(testKey, loader, TimeSpan.FromMilliseconds(1));
            });
            Assert.True(memoryCache.GetOrLoad(testKey, loader) <= 100);
        }
    }
}