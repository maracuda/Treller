using System.Linq;
using Xunit;
using SKBKontur.TaskManagerClient.Repository.Clients;
using SKBKontur.Treller.Tests.UnitWrappers;
using Assert = SKBKontur.Treller.Tests.UnitWrappers.Assert;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Repository
{
    public class GitlabClientTests : IntegrationTest
    {
        private IRepositoryClient gitlabClient;

        public GitlabClientTests() : base()
        {
            gitlabClient = container.Get<IRepositoryClientFactory>().CreateGitLabClient("584");
        }

        [Fact]
        public void TestBranches()
        {
            var allBranches = gitlabClient.SelectAllBranches();
            Assert.True(allBranches.Length > 2);
            var releaseBranch = allBranches.FirstOrDefault(x => x.Name == "release");
            Assert.IsNotNull(releaseBranch);
            Assert.IsNotNull(releaseBranch.Commit);
            Assert.IsNotNull(releaseBranch.Commit.Id);
            Assert.IsNotNull(releaseBranch.Commit.Message);
            Assert.IsNotNull(releaseBranch.Commit.Committer_name);
            Assert.IsNotNull(releaseBranch.Commit.Committer_email);
            Assert.IsNotNull(releaseBranch.Commit.Committed_date);
        }

        [Fact]
        public void TestLastCommitAtBranch()
        {
            var lastCommits = gitlabClient.SelectLastBranchCommits("release", 1, 100);
            Assert.True(lastCommits.Length == 100);
            var lastCommit = lastCommits.First();
            Assert.IsNotNull(lastCommit.Id);
            Assert.IsNotNull(lastCommit.Short_id);
            Assert.IsNotNull(lastCommit.Title);
            Assert.IsNotNull(lastCommit.Message);
            Assert.IsNotNull(lastCommit.Author_email);
            Assert.IsNotNull(lastCommit.Author_name);
            Assert.IsNotNull(lastCommit.Created_at);
        }
    }
}