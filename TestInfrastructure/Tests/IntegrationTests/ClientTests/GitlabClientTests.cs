using SKBKontur.TaskManagerClient.Repository.Clients;
using SKBKontur.Treller.Tests.UnitWrappers;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.ClientTests
{
    public class GitlabClientTests : IntegrationTest
    {
        private IRepositoryClient gitlabClient;

        public override void SetUp()
        {
            base.SetUp();

            gitlabClient = container.Get<IRepositoryClientFactory>().CreateGitLabClient("584");
        }

        [MyTest]
        public void TestBranches()
        {
            Assert.True(gitlabClient.SelectAllBranches().Length > 2);
        }
    }
}