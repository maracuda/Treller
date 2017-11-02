using RepositoryHooks.BranchNotification;
using Xunit;

namespace Tests.Tests.IntegrationTests.RepositoryHooks
{
    public class BranchNotificatorTests : IntegrationTest
    {
        private readonly IBranchNotificator branchNotificator;

        public BranchNotificatorTests()
        {
            branchNotificator = container.Get<IBranchNotificator>();
        }

        [Fact]
        public void SendNotificationsAboutOldBranches()
        {
            branchNotificator.NotifyCommitersAboutOldBranches();
        }

        [Fact]
        public void DeleteMergedBranches()
        {
            branchNotificator.DeleteMergedBranchesAndNotifyCommiters();
        }
    }
}