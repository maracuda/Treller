using System;
using ProcessStats.Incidents;
using Xunit;

namespace Tests.Tests.IntegrationTests.ProcessStats
{
    public class WicStatsCrawlerTest : IntegrationTest
    {
        private readonly IIncidentsStatsCrawler incidentsStatsCrawler;

        public WicStatsCrawlerTest()
        {
            incidentsStatsCrawler = container.Get<IIncidentsStatsCrawler>();
        }

        [Fact]
        public void AbleToCollectStats()
        {
            var stats = incidentsStatsCrawler.Collect(new DateTime(2017, 10, 2));
            Assert.True(stats.IncomingCount > 0);
        }
    }
}