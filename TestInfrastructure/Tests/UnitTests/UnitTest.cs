using NUnit.Framework;
using SKBKontur.Treller.Tests.MockWrappers;

namespace SKBKontur.Treller.Tests.Tests.UnitTests
{
    [TestFixture]
    public abstract class UnitTest
    {
        protected MyMock mock;

        [SetUp]
        public virtual void SetUp()
        {
            mock = new MyMock();
        }

        [TearDown]
        public virtual void TearDown()
        {
            mock.Dispose();
            mock.RunAll();
        }
    }
}