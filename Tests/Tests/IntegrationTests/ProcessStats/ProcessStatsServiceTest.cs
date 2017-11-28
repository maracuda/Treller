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
        public void CollectIncidentsAndBattles()
        {
            var dates = new[] { new DateTime(2017, 11, 17) };
            foreach (var dateTime in dates)
            {
                processStatsService.CollectAndPublishIncidentsStats(dateTime);
            }
        }
    }
}