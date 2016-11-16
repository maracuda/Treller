using Xunit;
using SKBKontur.TaskManagerClient.Repository.BusinessObjects;
using SKBKontur.Treller.Tests.Tests.UnitTests;
using SKBKontur.Treller.Tests.UnitWrappers;
using Assert = SKBKontur.Treller.Tests.UnitWrappers.Assert;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Repository
{
    public class CommitTest : UnitTest
    {
        [Theory]
        [InlineData("Merge branch 'ui/videos' into release", "ui/videos", "release")]
        [InlineData("Merge remote-tracking branch 'origin/release' into bill_info/cert_applications", "release", "bill_info/cert_applications")]
        [InlineData("Merge branch 'release' of https://git.skbkontur.ru/billy/billy into release", "release", "release")]
        [InlineData("Merge branch 'bill_info/clients_highlight' into 'release'", "bill_info/clients_highlight", "release")]
        public void TestIsMerge(string title, string fromBranch, string toBranch)
        {
            var commit = GenerateCommit(title);
            Assert.True(commit.IsMerge());
            Assert.True(commit.IsMerge(fromBranch, toBranch));
            Assert.True(commit.IsMerge(toBranch));
        }

        [Theory]
        [InlineData("Merge branch 'ui/videos' into release", "ui/videos")]
        [InlineData("Merge branch 'bill_info/clients_highlight' into 'release'", "bill_info/clients_highlight")]
        public void TestParseFromBranchNameSuccessfully(string title, string toBranch)
        {
            var commit = GenerateCommit(title);
            var actual = commit.ParseFromBranchName();
            Assert.True(actual.HasValue);
            Assert.AreEqual(toBranch, actual.Value);
        }

        [Fact]
        public void TestParseFromBranchNameFail()
        {
            var commit = GenerateCommit("bla bla bla");
            var actual = commit.ParseFromBranchName();
            Assert.False(actual.HasValue);
        }


        private static Commit GenerateCommit(string title)
        {
            return new Commit
            {
                Id = "79d6fea771d7dd84a1df4d52d317b1411cd21bb9",
                Short_id = "79d6fea7",
                Title = title,
                Author_name = "Ivan Sosnin",
                Author_email = "isosnin@skbkontur.ru",
                Message = "Merge branch 'ui/videos' into release\n"
            };
        }
    }
}