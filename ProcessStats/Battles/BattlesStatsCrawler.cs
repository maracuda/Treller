using System.Collections.Generic;
using TaskManagerClient;

namespace ProcessStats.Battles
{
    public class BattlesStatsCrawler : IBattlesStatsCrawler
    {
        private readonly IBugTrackerClient bugTrackerClient;
        private readonly ISubProductsRef subProductsRef;

        public BattlesStatsCrawler(
            IBugTrackerClient bugTrackerClient,
            ISubProductsRef subProductsRef)
        {
            this.bugTrackerClient = bugTrackerClient;
            this.subProductsRef = subProductsRef;
        }

        public BattlesStats Collect()
        {
            var unassignedCount = bugTrackerClient.GetFilteredCount("#Billy #Battle State: -Resolved Assignee: Unassigned");
            var subProductIds = subProductsRef.GetSubproductIds();
            var subProductsStatsList = new List<SubProductBattleStats>();
            foreach (var subProductId in subProductIds)
            {
                var productName = subProductsRef.GetSubProductName(subProductId);
                var subProductUnassignedCount = bugTrackerClient.GetFilteredCount($"project: Billing Type: Battle State: Open, Reopened Подпродукт: {{{productName}}} Assignee: Unassigned");
                var subProductTotalCount = bugTrackerClient.GetFilteredCount($"project: Billing Type: Battle State: Open, Reopened Подпродукт: {{{productName}}}");
                subProductsStatsList.Add(new SubProductBattleStats
                {
                    UnassignedCount = subProductUnassignedCount,
                    TotalCount = subProductTotalCount,
                    SubProductId = subProductId
                });
            }

            var fuckupsOrdersTotalCount = bugTrackerClient.GetFilteredCount("project: fuckups Teams: Billing.Orders");

            return new BattlesStats
            {
                UnassignedCount = unassignedCount,
                SubProducStats = subProductsStatsList.ToArray(),
                SubProductsStats = new []
                {
                    new SubProductFuckupStats
                    {
                        SubProductId = "Orders",
                        TotalCount = fuckupsOrdersTotalCount
                    }
                }
            };
        }
    }
}