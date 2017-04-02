using SKBKontur.Treller.Serialization;
using SKBKontur.Treller.Storage.FileStorage;
using Xunit;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Storages
{
    public class CollectionsStorageTest : IntegrationTest
    {
        private readonly CollectionsStorage<int> collectionsStorage;

        public CollectionsStorageTest()
        {
            collectionsStorage = new CollectionsStorage<int>(container.Get<IJsonSerializer>(), container.Get<IFileSystemHandler>());
        }

        ~CollectionsStorageTest()
        {
            collectionsStorage.Clear();
        }

        [Fact]
        public void TestPutAndGet()
        {
            var items = new[] {27, 349, 929};
            collectionsStorage.Put(items);
            var actual = collectionsStorage.GetAll();
            Assert.Equal(items, actual);
        }

        [Fact]
        public void TestDoublePut()
        {
            var items1 = new[] { 27, 349, 929 };
            var items2 = new[] { 4945, 827, 828 };
            collectionsStorage.Put(items1);
            collectionsStorage.Put(items2);
            var actual = collectionsStorage.GetAll();
            Assert.Equal(items2, actual);
        }

        [Fact]
        public void TestAppend()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            collectionsStorage.Append(12);
            var actual = collectionsStorage.GetAll();
            Assert.Equal(new[] { 27, 349, 929, 12 }, actual);
        }

        [Fact]
        public void TestGetEmpty()
        {
            var actual = collectionsStorage.GetAll();
            Assert.Equal(new int[0], actual);
        }

        [Fact]
        public void TestGetByIndex()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            var actual = collectionsStorage.Get(1);
            Assert.Equal(349, actual);
        }

        [Fact]
        public void TestRemoveAt()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            collectionsStorage.RemoveAt(1);
            var actual = collectionsStorage.GetAll();
            Assert.Equal(new[] { 27, 929 }, actual);
        }

        [Fact]
        public void TestClear()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            collectionsStorage.Clear();
            var actual = collectionsStorage.GetAll();
            Assert.Equal(new int[0], actual);
        }
    }
}