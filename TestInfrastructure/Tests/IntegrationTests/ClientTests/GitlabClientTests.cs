using SKBKontur.TaskManagerClient.Repository.Clients;
using SKBKontur.TestInfrastructure.UnitWrappers;

namespace SKBKontur.TestInfrastructure.Tests.IntegrationTests.ClientTests
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