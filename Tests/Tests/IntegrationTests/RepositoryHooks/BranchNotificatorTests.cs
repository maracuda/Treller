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
        public void SendNotifications()
        {
            branchNotificator.NotifyCommitersAboutOldBranches();
        }
    }
}