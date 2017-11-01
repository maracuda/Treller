using System;
using MessageBroker;
using Xunit;

namespace Tests.Tests.IntegrationTests.ProcessStats
{
    public class SpreadsheetProducerTest : IntegrationTest
    {
        private readonly ISpreadsheetsMessageProducer spreadsheetProducer;

        public SpreadsheetProducerTest()
        {
            spreadsheetProducer = container.Get<ISpreadsheetsMessageProducer>();
        }

        [Fact]
        public void AbleToAppendRowToTestDocument()
        {
            var date = new DateTime(2017, 10, 31);
            spreadsheetProducer.Publish("1-WJBhjaJpr3qb0_P88995q1MWcGumKy6xgvoT5dSm4k", 0, new object[] {1, 2, 3, "zzzzz", date} );
        }
    }
}