using System.IO;
using System.Linq;
using System.Text;
using ProcessStats.Dev;
using TaskManagerClient;
using Xunit;

namespace Tests.Tests.IntegrationTests.ProcessStats
{
    public class ReportBuilderTest : IntegrationTest
    {
        private readonly ReportBuilder reportBuilder;

        public ReportBuilderTest()
        {
            reportBuilder = container.Get<ReportBuilder>();
        }

        [Fact]
        public void ReportBuildsSomething()
        {
            const string billingDeliveredBoardId = "58d22275df59d0815216e1f0";
            var result = reportBuilder.BuildDetalization(billingDeliveredBoardId);
            WriteStatsToFile(result);
        }

        private static void WriteStatsToFile(CardsAggregationStatsModel cardsAggregationStats)
        {
            const string reportPath = "statsDetalizationReport.csv";
            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }

            var strBuilder = new StringBuilder();
            strBuilder.AppendLine("CardName;CycleTime;");
            foreach (var cardStats in cardsAggregationStats.CardsStats)
            {
                strBuilder.AppendLine($"{cardStats.CardName};{cardStats.CycleTime};");
            }
            strBuilder.AppendLine(string.Empty);
            strBuilder.AppendLine($"Average cycle time;{cardsAggregationStats.AverageCycleTime};");
            strBuilder.AppendLine($"Longest cycle time;{cardsAggregationStats.LongestCycleCardStats.CycleTime}, cardName {cardsAggregationStats.LongestCycleCardStats.CardName};");
            strBuilder.AppendLine($"Shortest cycle time;{cardsAggregationStats.ShortestCycleCardStats.CycleTime}, cardName {cardsAggregationStats.ShortestCycleCardStats.CardName};");

            File.WriteAllText(reportPath, strBuilder.ToString());
        }
    }
}