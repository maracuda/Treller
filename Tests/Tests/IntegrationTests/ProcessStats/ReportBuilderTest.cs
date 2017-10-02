using System;
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
        public void BuildReportForBillingDeliveryBoard()
        {
            const string billingDeliveredBoardId = "58d22275df59d0815216e1f0";
            const string waitForFeedbackListId = "58d2295834d4e5619d71d008";
            const string doneListId = "58d2296edeed2d3c3e25acdd";
            var detalization = reportBuilder.BuildDetalization(billingDeliveredBoardId, doneListId, waitForFeedbackListId);
            var lists = container.Get<ITaskManagerClient>().GetBoardLists(billingDeliveredBoardId);
            var listNames = lists.Select(l => l.Name).ToArray();
            var listNameToIdIndex = lists.ToDictionary(l => l.Name, l => l.Id);
            WriteAggregationStatsToFile("fullStatsReport", detalization, listNames, listNameToIdIndex);
            WriteAggregationStatsToFile($"statsBy{TaskQueuesKnownLabels.ProductQueueLabel.Name}Report", detalization.FilterBy(TaskQueuesKnownLabels.ProductQueueLabel), listNames, listNameToIdIndex);
            WriteAggregationStatsToFile($"statsBy{TaskQueuesKnownLabels.CrmQueueLabel.Name}Report", detalization.FilterBy(TaskQueuesKnownLabels.CrmQueueLabel), listNames, listNameToIdIndex);
            WriteAggregationStatsToFile($"statsBy{TaskQueuesKnownLabels.InfrastructureQueueLabel.Name}Report", detalization.FilterBy(TaskQueuesKnownLabels.InfrastructureQueueLabel), listNames, listNameToIdIndex);
            WriteAggregationStatsToFile($"statsBy{TaskQueuesKnownLabels.SupportQueueLabel.Name}Report", detalization.FilterBy(TaskQueuesKnownLabels.SupportQueueLabel), listNames, listNameToIdIndex);
        }

        [Theory]
        [InlineData("motocycle", "58874dd11d04205448b0435d", "58875668c6677244cf4ccd03")]
        [InlineData("portalAuth", "5976f7264ba9e718190cc5d0", "5976f74a5e8b7544b1fc0738")]
        [InlineData("market", "595b48916ed422e278c6c9f8", "595b48ae0cf6ee288bcba874")]
        [InlineData("discounts", "59ad172066c11f4cccf3e894", "59ad1c5344878b519796fa37")]
        public void BuildReportForDirectionBoards(string boardName, string boardId, string doneListId)
        {
            var lists = container.Get<ITaskManagerClient>().GetBoardLists(boardId);
            var listNames = lists.Select(l => l.Name).ToArray();
            var listNameToIdIndex = lists.ToDictionary(l => l.Name, l => l.Id);
            var detalization = reportBuilder.BuildDetalization(boardId, doneListId);
            WriteAggregationStatsToFile($"{boardName}StatsReport", detalization, listNames, listNameToIdIndex);
        }

        private static void WriteAggregationStatsToFile(string reportName, CardsAggregationStatsModel cardsAggregationStats, string[] listNames, Dictionary<string, string> listNameToIdIndex)
        {
            var reportPath = $"{reportName}.csv";
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
            strBuilder.AppendLine("Full aggregation stats;");
            WriteAgregationStats(strBuilder, cardsAggregationStats.FullAggregationStats);
            strBuilder.AppendLine("S tasks aggregation stats;");
            WriteAgregationStats(strBuilder, cardsAggregationStats.SAggregationStats);
            strBuilder.AppendLine("M tasks aggregation stats;");
            WriteAgregationStats(strBuilder, cardsAggregationStats.MAggregationStats);
            strBuilder.AppendLine("L tasks aggregation stats;");
            WriteAgregationStats(strBuilder, cardsAggregationStats.LAggregationStats);
            strBuilder.AppendLine("XL tasks aggregation stats;");
            WriteAgregationStats(strBuilder, cardsAggregationStats.XLAggregationStats);
            File.WriteAllBytes(reportPath, Encoding.GetEncoding("windows-1251").GetBytes(strBuilder.ToString()));
            //File.WriteAllText(reportPath, strBuilder.ToString());
        }

        private static void WriteAgregationStats(StringBuilder strBuilder, AggregationTimeStats cardsAggregationStats)
        {
            if (cardsAggregationStats.AreEmpty())
                return;

            strBuilder.AppendLine($"Average cycle time;{FormatTimeSpan(cardsAggregationStats.AverageTime)}");
            strBuilder.AppendLine($"Longest cycle time;{FormatTimeSpan(cardsAggregationStats.LongestTimeCard.CycleTime)}, cardName {cardsAggregationStats.LongestTimeCard.Name};");
            strBuilder.AppendLine($"Shortest cycle time;{FormatTimeSpan(cardsAggregationStats.ShortestTimeCard.CycleTime)}, cardName {cardsAggregationStats.ShortestTimeCard.Name};");
        }

        private static void WriteCardStats(StringBuilder strBuilder, CardStatsModel cardStats, string[] listNames, Dictionary<string, string> listNameToIdIndex)
        {
            strBuilder.Append($"{cardStats.Name};{cardStats.Size};{FormatTimeSpan(cardStats.CycleTime)};");
            foreach (var listName in listNames)
            {
                var listId = listNameToIdIndex[listName];
                if (cardStats.ListStats.ContainsKey(listId))
                {
                    strBuilder.Append($"{FormatTimeSpan(cardStats.ListStats[listId])}");
                }
                strBuilder.Append(";");
            }
            strBuilder.AppendLine();
        }

        private static void WriteReportHeader(StringBuilder strBuilder, string[] listNames)
        {
            strBuilder.Append("Name;Size;CycleTime;");
            foreach (var listName in listNames)
            {
                strBuilder.Append($"{listName};");
            }
            strBuilder.AppendLine();
        }

        private static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Days}";
        }
    }
}