using System;
using TaskManagerClient.Repository;
using Xunit;

namespace Tests.Tests.IntegrationTests.Repository
{
    public class RepositoryTest : IntegrationTest
    {
        private readonly IRepository repository;

        public RepositoryTest()
        {
            repository = container.Get<IRepository>();
        }

        [Fact]
        public void TestSearchForOldBranches()
        {
            var actual = repository.SearchForOldBranches(TimeSpan.FromDays(1));
            Assert.True(actual.Length > 10);
        }
    }
}