using System.Collections.Generic;
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
            var lists = container.Get<ITaskManagerClient>().GetBoardLists(billingDeliveredBoardId);
            var listNames = lists.Select(l => l.Name).ToArray();
            var listNameToIdIndex = lists.ToDictionary(l => l.Name, l => l.Id);
            WriteStatsToFile(result, listNames, listNameToIdIndex);
        }

        private static void WriteStatsToFile(CardsAggregationStatsModel cardsAggregationStats, string[] listNames, Dictionary<string, string> listNameToIdIndex)
        {
            const string reportPath = "statsDetalizationReport.csv";
            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }

            var strBuilder = new StringBuilder();
            WriteReportHeader(strBuilder, listNames);
            foreach (var cardStats in cardsAggregationStats.CardsStats)
            {
                WriteCardStats(strBuilder, cardStats, listNames, listNameToIdIndex);
            }
            strBuilder.AppendLine(string.Empty);
            WriteAgregationStats(strBuilder, cardsAggregationStats);
            File.WriteAllText(reportPath, strBuilder.ToString());
        }

        private static void WriteAgregationStats(StringBuilder strBuilder, CardsAggregationStatsModel cardsAggregationStats)
        {
            strBuilder.AppendLine($"Average cycle time;{cardsAggregationStats.AverageCycleTime};");
            strBuilder.AppendLine($"Longest cycle time;{cardsAggregationStats.LongestCycleCardStats.CycleTime}, cardName {cardsAggregationStats.LongestCycleCardStats.CardName};");
            strBuilder.AppendLine($"Shortest cycle time;{cardsAggregationStats.ShortestCycleCardStats.CycleTime}, cardName {cardsAggregationStats.ShortestCycleCardStats.CardName};");
        }

        private static void WriteCardStats(StringBuilder strBuilder, CardStatsModel cardStats, string[] listNames, Dictionary<string, string> listNameToIdIndex)
        {
            strBuilder.Append($"{cardStats.CardName};{cardStats.CycleTime};");
            foreach (var listName in listNames)
            {
                var listId = listNameToIdIndex[listName];
                if (cardStats.ListStats.ContainsKey(listId))
                {
                    strBuilder.Append($"{cardStats.ListStats[listId]}");
                }
                strBuilder.Append(";");
            }
            strBuilder.AppendLine();
        }

        private static void WriteReportHeader(StringBuilder strBuilder, string[] listNames)
        {
            strBuilder.Append("CardName;CycleTime;");
            foreach (var listName in listNames)
            {
                strBuilder.Append($"{listName};");
            }
            strBuilder.AppendLine();
        }
    }
}