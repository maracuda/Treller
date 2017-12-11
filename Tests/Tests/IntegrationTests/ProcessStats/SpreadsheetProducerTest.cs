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
            spreadsheetProducer.Append("1-WJBhjaJpr3qb0_P88995q1MWcGumKy6xgvoT5dSm4k", "Sheet1", GenerageDataRow());
        }

        [Fact]
        public void AbleToRewriteSheet()
        {
            var dataRows = new[]
            {
                GenerageDataRow(), GenerageDataRow(), GenerageDataRow()
            };
            spreadsheetProducer.Rewrite("1-WJBhjaJpr3qb0_P88995q1MWcGumKy6xgvoT5dSm4k", "Sheet1", dataRows);
        }

        private static DataRow GenerageDataRow()
        {
            return DataRow.Create(DataGenerator.GenInt(), DataGenerator.GenInt(), DataGenerator.GenInt(), DataGenerator.GenEnglishString(15), new DateTime(2017, 10, 31));
        }
    }
}