using TaskManagerClient;

namespace ProcessStats.Battles
{
    public class BattlesStatsCrawler : IBattlesStatsCrawler
    {
        private readonly IBugTrackerClient bugTrackerClient;

        public BattlesStatsCrawler(IBugTrackerClient bugTrackerClient)
        {
            this.bugTrackerClient = bugTrackerClient;
        }

        public BattlesStats Collect()
        {
            var unassignedCount = bugTrackerClient.GetFilteredCount("#Billy #Battle State: -Resolved Assignee: Unassigned");
            return new BattlesStats
            {
                UnassignedCount = unassignedCount
            };
        }
    }
}