using System;
using Xunit;

namespace Tests.Tests.UnitTests
{
    public class AssertionTests : UnitTest
    {
        [Fact]
        public void AssertsShouldWorkCorrectly()
        {
            var id = Guid.NewGuid();

            UnitWrappers.Assert.AreEqual(1, 1);
            UnitWrappers.Assert.AreNotEqual(new SomeType { Id = id }, new SomeType { Id = id });
            UnitWrappers.Assert.AreDeepEqual(new SomeType { Id = id }, new SomeType { Id = id });
            UnitWrappers.Assert.False(false);
            UnitWrappers.Assert.True(true);
            UnitWrappers.Assert.Throws(typeof(Exception), () => { throw new Exception("mess"); }, "mess");
            UnitWrappers.Assert.Throws<Exception>(() => { throw new Exception("mess"); }, "mess");
        }

        public class SomeType
        {
            public Guid Id { get; set; }
        }
    }
}