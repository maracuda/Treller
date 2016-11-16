using System;
using Xunit;
using SKBKontur.TaskManagerClient.Repository;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Repository
{
    public class RepositoryTest : IntegrationTest
    {
        private IRepository repository;

        public RepositoryTest() : base()
        {
            repository = container.Get<IRepository>();
        }

        [Fact]
        public void TestSearchForOldBranches()
        {
            var actual = repository.SearchForOldBranches(TimeSpan.FromDays(1));
            Assert.True(actual.Length > 10);
        }

        [Fact]
        public void TestSearchForMergedBranches()
        {
            var actual = repository.SearchForMergedToReleaseBranches(TimeSpan.FromDays(10));
            Assert.True(actual.Length > 0);
        }
    }
}