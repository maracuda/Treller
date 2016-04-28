using SKBKontur.TaskManagerClient.Repository.BusinessObjects;
using SKBKontur.Treller.Tests.Tests.UnitTests;
using SKBKontur.Treller.Tests.UnitWrappers;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Repository
{
    public class RepoCommitTest : UnitTest
    {
        private readonly Commit testCommit = new Commit
        {
            Id = "79d6fea771d7dd84a1df4d52d317b1411cd21bb9",
            Short_id = "79d6fea7",
            Title = "Merge branch 'ui/videos' into release",
            Author_name = "Ivan Sosnin",
            Author_email = "isosnin@skbkontur.ru",
            Message = "Merge branch 'ui/videos' into release\n"
        };

        [MyTest]
        public void TestIsMerge()
        {
            Assert.True(testCommit.IsMerge());
            Assert.True(testCommit.IsMerge("ui/videos", "release"));
        }
    }
}