using System;
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

        public BattlesStats Collect(DateTime date)
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