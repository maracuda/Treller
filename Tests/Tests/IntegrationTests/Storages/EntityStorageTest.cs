using Xunit;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Storages
{
    public class EntityStorageTest : IntegrationTest
    {
        private EntityStorage entityStorage;

        public EntityStorageTest() : base()
        {
            entityStorage = container.Get<EntityStorage>();
        }

        ~EntityStorageTest()
        {
            entityStorage.DeleteAll();
        }

        [Fact]
        public void TestPutAndGet()
        {
            entityStorage.Put(5);
            var actual = entityStorage.Get<int>();
            Assert.Equal(5, actual);
        }

        [Fact]
        public void TestPutAndDelete()
        {
            entityStorage.Put(5);
            entityStorage.Delete<int>();
            var actual = entityStorage.Get<int>();
            Assert.Equal(0, actual);
        }

        [Fact]
        public void TestDoublePut()
        {
            entityStorage.Put(5);
            entityStorage.Put(8);
            var actual = entityStorage.Get<int>();
            Assert.Equal(8, actual);
        }

        [Fact]
        public void TestGetUnexistent()
        {
            var actual = entityStorage.Get<int>();
            Assert.Equal(0, actual);
        }

        [Fact]
        public void TextDeleteAll()
        {
            entityStorage.Put(5);
            entityStorage.Put(10L);
            entityStorage.Put(20D);
            entityStorage.DeleteAll();
            Assert.Equal(0, entityStorage.Get<int>());
            Assert.Equal(0L, entityStorage.Get<long>());
            Assert.Equal(0D, entityStorage.Get<double>());
        }
    }
}