using System;
using NUnit.Framework;
using SKBKontur.TaskManagerClient.Repository;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Repository
{
    public class RepositoryTest : IntegrationTest
    {
        private IRepository repository;

        public override void SetUp()
        {
            base.SetUp();

            repository = container.Get<IRepository>();
        }

        [Test]
        public void TestSearchForOldBranches()
        {
            var actual = repository.SearchForOldBranches(TimeSpan.FromDays(1));
            Assert.True(actual.Length > 10);
        }

        [Test]
        public void TestSearchForMergedBranches()
        {
            var actual = repository.SearchForMergedToReleaseBranches(TimeSpan.FromDays(10));
            Assert.True(actual.Length > 0);
        }
    }
}