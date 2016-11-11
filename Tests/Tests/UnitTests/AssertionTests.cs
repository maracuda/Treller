using System;
using SKBKontur.Treller.Tests.UnitWrappers;

namespace SKBKontur.Treller.Tests.Tests.UnitTests
{
    public class AssertionTests : UnitTest
    {
        [MyTest]
        public void AssertsShouldWorkCorrectly()
        {
            var id = Guid.NewGuid();

            Assert.AreEqual(1, 1);
            Assert.AreNotEqual(new SomeType { Id = id }, new SomeType { Id = id });
            Assert.AreDeepEqual(new SomeType { Id = id }, new SomeType { Id = id });
            Assert.False(false);
            Assert.True(true);
            Assert.Throws(typeof(Exception), () => { throw new Exception("mess"); }, "mess");
            Assert.Throws<Exception>(() => { throw new Exception("mess"); }, "mess");
        }

        public class SomeType
        {
            public Guid Id { get; set; }
        }
    }
}