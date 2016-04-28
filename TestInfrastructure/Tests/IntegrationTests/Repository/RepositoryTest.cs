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
        public void TestSelectOldBranches()
        {
            var actual = repository.SearchForOldBranches(TimeSpan.FromDays(1));
            var veryOldBranches = repository.SearchForOldBranches(TimeSpan.FromDays(90));
            Assert.True(actual.Length > 10);
        }
    }
}