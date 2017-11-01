using System.Linq;
using TaskManagerClient.Repository.Clients;
using Xunit;
using Assert = Tests.UnitWrappers.Assert;

namespace Tests.Tests.IntegrationTests.Repository
{
    public class GitlabClientTests : IntegrationTest
    {
        private readonly IRepositoryClient gitlabClient;

        public GitlabClientTests()
        {
            gitlabClient = container.Get<IRepositoryClientFactory>().CreateGitLabClient("584");
        }

        [Fact]
        public void AbleToSelectBranches()
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
        public void AbleToFindLastCommitAtBranch()
        {
            var lastCommits = gitlabClient.SelectLastCommits("release", 1, 100);
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

        [Fact]
        public void AbleToCrateAndDeleteBranch()
        {
            const string testBranchName = "testFeature";
            var newBranch = gitlabClient.CreateBranch(testBranchName, "release");
            Assert.IsNotNull(newBranch);
            Assert.AreEqual(newBranch.Name, testBranchName);

            gitlabClient.DeleteBranch(testBranchName);
            var actualBranches = gitlabClient.SelectAllBranches();
            Assert.AreEqual(0, actualBranches.Count(b => b.Name.Equals(testBranchName)));

        }
    }
}