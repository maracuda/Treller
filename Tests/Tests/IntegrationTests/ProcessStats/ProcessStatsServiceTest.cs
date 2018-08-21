using System;
using ProcessStats;
using Xunit;

namespace Tests.Tests.IntegrationTests.ProcessStats
{
    public class ProcessStatsServiceTest : IntegrationTest
    {
        private readonly IProcessStatsService processStatsService;

        public ProcessStatsServiceTest()
        {
            processStatsService = container.Get<IProcessStatsService>();
        }

        [Fact]
        public void CollectIncidents()
        {
            var dates = new[] { new DateTime(2017, 11, 29) };
            foreach (var dateTime in dates)
            {
                processStatsService.CollectAndPublishIncidentsStats(dateTime);
            }
        }

        [Fact]
        void CollectBattles()
        {
            processStatsService.CollectAndPublishBattlesStats();
        }

        [Fact]
        public void CollectProjectStats()
        {
            processStatsService.BuildAllAndDeliverToManagers();
        }
    }
}