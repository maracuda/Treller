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
            spreadsheetProducer.Publish("1-WJBhjaJpr3qb0_P88995q1MWcGumKy6xgvoT5dSm4k", 0, "Sheet1", new []{"1", "2", "3"} );
        }
    }
}