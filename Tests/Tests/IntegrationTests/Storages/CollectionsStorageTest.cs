using Xunit;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Storages
{
    public class CollectionsStorageTest : IntegrationTest
    {
        private CollectionsStorage collectionsStorage;

        public CollectionsStorageTest() : base()
        {
            collectionsStorage = container.Get<CollectionsStorage>();
        }

        ~CollectionsStorageTest()
        {
            collectionsStorage.DeleteAll();
        }

        [Fact]
        public void TestPutAndGet()
        {
            var items = new[] {27, 349, 929};
            collectionsStorage.Put(items);
            var actual = collectionsStorage.GetAll<int>();
            Assert.Equal(items, actual);
        }

        [Fact]
        public void TestDoublePut()
        {
            var items1 = new[] { 27, 349, 929 };
            var items2 = new[] { 4945, 827, 828 };
            collectionsStorage.Put(items1);
            collectionsStorage.Put(items2);
            var actual = collectionsStorage.GetAll<int>();
            Assert.Equal(items2, actual);
        }

        [Fact]
        public void TestAppend()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            collectionsStorage.Append(12);
            var actual = collectionsStorage.GetAll<int>();
            Assert.Equal(new[] { 27, 349, 929, 12 }, actual);
        }

        [Fact]
        public void TestGetEmpty()
        {
            var actual = collectionsStorage.GetAll<int>();
            Assert.Equal(new int[0], actual);
        }

        [Fact]
        public void TestGetByIndex()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            var actual = collectionsStorage.Get<int>(1);
            Assert.Equal(349, actual);
        }

        [Fact]
        public void TestRemoveAt()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            collectionsStorage.RemoveAt<int>(1);
            var actual = collectionsStorage.GetAll<int>();
            Assert.Equal(new[] { 27, 929 }, actual);
        }

        [Fact]
        public void TestDelete()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            collectionsStorage.Delete<int>();
            var actual = collectionsStorage.GetAll<int>();
            Assert.Equal(new int[0], actual);
        }

        [Fact]
        public void TestDeleteAll()
        {
            var ints = new[] { 27, 349, 929 };
            var longs = new[] {3838L, 772L, 9393L};
            collectionsStorage.Put(ints);
            collectionsStorage.Put(longs);
            collectionsStorage.DeleteAll();
            Assert.Equal(new int[0], collectionsStorage.GetAll<int>());
            Assert.Equal(new long[0], collectionsStorage.GetAll<long>());
        }

    }
}