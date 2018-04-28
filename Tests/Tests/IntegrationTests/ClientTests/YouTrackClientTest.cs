using TaskManagerClient;
using Xunit;

namespace Tests.Tests.IntegrationTests.ClientTests
{
    public class YouTrackClientTest : IntegrationTest
    {
        private readonly IBugTrackerClient bugTrackerClient;

        public YouTrackClientTest()
        {
            bugTrackerClient = container.Get<IBugTrackerClient>();
        }

        [Fact]
        public void AbleToFilterBattles()
        {
            const string filter = "project: Billy Type: Battle created: 2017-07-22 ..2017-07-24";
            var actualIssues = bugTrackerClient.GetFiltered(filter);
            var actualIssuesCount = bugTrackerClient.GetFilteredCount(filter);
            Assert.NotEmpty(actualIssues);
            Assert.Equal(actualIssues.Length, actualIssuesCount);
        }

        [Fact]
        public void AbleToFilterWithShortcuts()
        {
            const string filter = "#Billy #Battle State: -Resolved Assignee: Unassigned";
            var actualIssuesCount = bugTrackerClient.GetFilteredCount(filter);
            Assert.True(actualIssuesCount >= 0);
        }
    }
}