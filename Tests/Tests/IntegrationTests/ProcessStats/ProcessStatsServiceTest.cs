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
            var dates = new[] { new DateTime(2018, 1, 7), new DateTime(2018, 1, 8), new DateTime(2018, 1, 9) };
            foreach (var dateTime in dates)
            {
                processStatsService.CollectAndPublishBattlesStats(dateTime);
            }
        }

        [Fact]
        public void CollectProjectStats()
        {
            processStatsService.BuildAllAndDeliverToManagers();
        }
    }
}