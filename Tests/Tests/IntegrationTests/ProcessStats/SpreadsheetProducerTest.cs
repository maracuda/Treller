using System;
using System.Linq;
using MessageBroker;
using ProcessStats.Dev;
using Xunit;

namespace Tests.Tests.IntegrationTests.ProcessStats
{
    public class SpreadsheetProducerTest : IntegrationTest
    {
        private readonly ISpreadsheetsMessageProducer spreadsheetProducer;
        private readonly IStatsReportBuilder statsReportBuilder;

        public SpreadsheetProducerTest()
        {
            spreadsheetProducer = container.Get<ISpreadsheetsMessageProducer>();
            statsReportBuilder = container.Get<IStatsReportBuilder>();
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

        [Fact]
        public void AbleToPublishProjectMetricsToSPreadsheets()
        {
            ReportModel[] stats = statsReportBuilder.BuildForBillingDelivery();
            spreadsheetProducer.Rewrite("1HxfCoYYQsyevahb1qnHqjTgxVvA_zw_a8nM0ijE5Bm0", stats[0].Name, stats[0].Rows.Select(r => DataRow.Create(r.Values)).ToArray());
        }

        private static DataRow GenerageDataRow()
        {
            return DataRow.Create(DataGenerator.GenInt(), DataGenerator.GenInt(), DataGenerator.GenInt(), DataGenerator.GenEnglishString(15), new DateTime(2017, 10, 31));
        }
    }
}