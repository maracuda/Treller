using System;
using TaskManagerClient;

namespace ProcessStats.Battles
{
    public class BattlesStatsService : IBattlesStatsService
    {
        private readonly IBugTrackerClient bugTrackerClient;

        public BattlesStatsService(IBugTrackerClient bugTrackerClient)
        {
            this.bugTrackerClient = bugTrackerClient;
        }

        public BattlesStats GetStats(DateTime date)
        {
            var createdCount = bugTrackerClient.GetFilteredCount($"project: Billy Type: Battle created: {date:yyyy-MM-dd}");
            var fixedCount = bugTrackerClient.GetFilteredCount($"project: Billy Type: Battle resolved date: {date:yyyy-MM-dd}");
            var reopenCount = bugTrackerClient.GetFilteredCount($"project: Billy Type: Battle Регулярность: Периодически,Постоянно updated: {date:yyyy-MM-dd}");

            return new BattlesStats
            {
                Date = date,
                CreatedCount = createdCount,
                ReopenCount = reopenCount,
                FixedCount = fixedCount
            };
        }
    }
}