using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Storages
{
    public class CollectionsStorageTest : IntegrationTest
    {
        private CollectionsStorage collectionsStorage;

        public override void SetUp()
        {
            base.SetUp();

            collectionsStorage = container.Get<CollectionsStorage>();
        }

        public override void TearDown()
        {
            base.TearDown();

            collectionsStorage.DeleteAll();
        }

        [Test]
        public void TestPutAndGet()
        {
            var items = new[] {27, 349, 929};
            collectionsStorage.Put(items);
            var actual = collectionsStorage.GetAll<int>();
            CollectionAssert.AreEqual(items, actual);
        }

        [Test]
        public void TestDoublePut()
        {
            var items1 = new[] { 27, 349, 929 };
            var items2 = new[] { 4945, 827, 828 };
            collectionsStorage.Put(items1);
            collectionsStorage.Put(items2);
            var actual = collectionsStorage.GetAll<int>();
            CollectionAssert.AreEqual(items2, actual);
        }

        [Test]
        public void TestAppend()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            collectionsStorage.Append(12);
            var actual = collectionsStorage.GetAll<int>();
            CollectionAssert.AreEqual(new[] { 27, 349, 929, 12 }, actual);
        }

        [Test]
        public void TestGetEmpty()
        {
            var actual = collectionsStorage.GetAll<int>();
            CollectionAssert.AreEqual(new int[0], actual);
        }

        [Test]
        public void TestGetByIndex()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            var actual = collectionsStorage.Get<int>(1);
            Assert.AreEqual(349, actual);
        }

        [Test]
        public void TestRemoveAt()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            collectionsStorage.RemoveAt<int>(1);
            var actual = collectionsStorage.GetAll<int>();
            CollectionAssert.AreEqual(new[] { 27, 929 }, actual);
        }

        [Test]
        public void TestDelete()
        {
            var items = new[] { 27, 349, 929 };
            collectionsStorage.Put(items);
            collectionsStorage.Delete<int>();
            var actual = collectionsStorage.GetAll<int>();
            CollectionAssert.AreEqual(new int[0], actual);
        }

        [Test]
        public void TestDeleteAll()
        {
            var ints = new[] { 27, 349, 929 };
            var longs = new[] {3838L, 772L, 9393L};
            collectionsStorage.Put(ints);
            collectionsStorage.Put(longs);
            collectionsStorage.DeleteAll();
            CollectionAssert.AreEqual(new int[0], collectionsStorage.GetAll<int>());
            CollectionAssert.AreEqual(new long[0], collectionsStorage.GetAll<long>());
        }

    }
}