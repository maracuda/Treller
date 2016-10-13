using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Storages
{
    public class EntityStorageTest : IntegrationTest
    {
        private EntityStorage entityStorage;

        public override void SetUp()
        {
            base.SetUp();

            entityStorage = container.Get<EntityStorage>();
        }

        public override void TearDown()
        {
            entityStorage.DeleteAll();

            base.TearDown();
        }

        [Test]
        public void TestPutAndGet()
        {
            entityStorage.Put(5);
            var actual = entityStorage.Get<int>();
            Assert.AreEqual(5, actual);
        }

        [Test]
        public void TestPutAndDelete()
        {
            entityStorage.Put(5);
            entityStorage.Delete<int>();
            var actual = entityStorage.Get<int>();
            Assert.AreEqual(0, actual);
        }

        [Test]
        public void TestDoublePut()
        {
            entityStorage.Put(5);
            entityStorage.Put(8);
            var actual = entityStorage.Get<int>();
            Assert.AreEqual(8, actual);
        }

        [Test]
        public void TestGetUnexistent()
        {
            var actual = entityStorage.Get<int>();
            Assert.AreEqual(0, actual);
        }

        [Test]
        public void TextDeleteAll()
        {
            entityStorage.Put(5);
            entityStorage.Put(10L);
            entityStorage.Put(20D);
            entityStorage.DeleteAll();
            Assert.AreEqual(0, entityStorage.Get<int>());
            Assert.AreEqual(0L, entityStorage.Get<long>());
            Assert.AreEqual(0D, entityStorage.Get<double>());
        }
    }
}