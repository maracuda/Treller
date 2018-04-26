using System;
using MessageBroker.Bots;
using MessageBroker.Messages;
using Xunit;

namespace Tests.Tests.IntegrationTests.MessageBroker
{
    public class SpreadsheetsBotTest : IntegrationTest
    {
        private readonly ISpreadsheetsBot spreadsheetProducer;

        public SpreadsheetsBotTest()
        {
            spreadsheetProducer = container.Get<ISpreadsheetsBot>();
        }

        [Fact]
        public void AbleToAppendRowToTestDocument()
        {
            spreadsheetProducer.Publish(new Report
            {
                SpreadsheetId = "1-WJBhjaJpr3qb0_P88995q1MWcGumKy6xgvoT5dSm4k",
                SheetName = "Sheet1",
                DataRows = new []{ GenerageDataRow() },
                Type = ReportType.Diff
            });
        }

        [Fact]
        public void AbleToRewriteSheet()
        {
            var dataRows = new[]
            {
                GenerageDataRow(), GenerageDataRow(), GenerageDataRow()
            };
            spreadsheetProducer.Publish(new Report
            {
                SpreadsheetId = "1-WJBhjaJpr3qb0_P88995q1MWcGumKy6xgvoT5dSm4k",
                SheetName = "Sheet1",
                DataRows = dataRows,
                Type = ReportType.Full
            });
        }

        private static DataRow GenerageDataRow()
        {
            return DataRow.Create(DataGenerator.GenInt(), DataGenerator.GenInt(), DataGenerator.GenInt(), DataGenerator.GenEnglishString(15), new DateTime(2017, 10, 31));
        }
    }
}