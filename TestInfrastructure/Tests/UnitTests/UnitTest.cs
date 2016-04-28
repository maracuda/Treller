using NUnit.Framework;
using SKBKontur.TestInfrastructure.MockWrappers;

namespace SKBKontur.TestInfrastructure.Tests.UnitTests
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