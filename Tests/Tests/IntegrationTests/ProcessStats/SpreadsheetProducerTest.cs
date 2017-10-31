using System;
using ProcessStats;
using ProcessStats.SpreadsheetProducer;
using Xunit;

namespace Tests.Tests.IntegrationTests.ProcessStats
{
    public class SpreadsheetProducerTest : IntegrationTest
    {
        private readonly ISpreadsheetProducer spreadsheetProducer;

        public SpreadsheetProducerTest()
        {
            spreadsheetProducer = container.Get<ISpreadsheetProducer>();
        }

        [Fact]
        public void AbleToAppendRowToTestDocument()
        {
            var date = new DateTime(2017, 10, 31);
            spreadsheetProducer.Publish("1-WJBhjaJpr3qb0_P88995q1MWcGumKy6xgvoT5dSm4k", 0, new object[] {1, 2, 3, "zzzzz", date} );
        }

        [Fact]
        public void Able()
        {
            container.Get<IProcessStatsService>().CollectAndPublishBattlesStats();
        }
    }
}