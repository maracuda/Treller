using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SKBKontur.TaskManagerClient.Caching;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests
{
    public class MemoryCacheTest : IntegrationTest
    {
        private const string testKey = "testKey";
        private IMemoryCache memoryCache;

        public override void SetUp()
        {
            base.SetUp();

            memoryCache = container.Get<ICacheFactory>().CreateMemoryCache("test", TimeSpan.FromMilliseconds(500));
        }

        [Test]
        public void TestGetFromCacheValueType()
        {
            var value = 0;
            var loader = new Func<int>(() => ++value);

            Assert.AreEqual(1, memoryCache.GetOrLoad(testKey, loader));
            Assert.AreEqual(1, memoryCache.GetOrLoad(testKey, loader));
            Assert.AreEqual(1, memoryCache.GetOrLoad(testKey, loader));
        }

        [Test]
        public void TestGetFromCacheRefType()
        {
            var value = 0;
            var loader = new Func<string>(() => (++value).ToString());

            Assert.AreEqual("1", memoryCache.GetOrLoad(testKey, loader));
            Assert.AreEqual("1", memoryCache.GetOrLoad(testKey, loader));
            Assert.AreEqual("1", memoryCache.GetOrLoad(testKey, loader));
        }

        [Test]
        public void TestGetFromCacheWithGlobalTtl()
        {
            var value = 0;
            var loader = new Func<int>(() => ++value);

            Assert.AreEqual(1, memoryCache.GetOrLoad(testKey, loader));
            Thread.Sleep(00);
            Assert.AreEqual(2, memoryCache.GetOrLoad(testKey, loader));
        }

        [Test]
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

        [Test]
        public void TestConcurrentGet()
        {
            var value = 0;
            var loader = new Func<int>(() => ++value);

            Parallel.For(1, 100, number =>
            {
                memoryCache.GetOrLoad(testKey, loader, TimeSpan.FromMilliseconds(1));
            });
            Assert.LessOrEqual(memoryCache.GetOrLoad(testKey, loader), 100);
        }
    }
}