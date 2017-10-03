using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagerClient;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public class StatsReportBuilder : IStatsReportBuilder
    {
        private static readonly Encoding defaultReportEncoding = Encoding.GetEncoding("windows-1251");
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ICardsAggregator cardsAggregator;

        public StatsReportBuilder(
            ITaskManagerClient taskManagerClient,
            ICardsAggregator cardsAggregator)
        {
            this.taskManagerClient = taskManagerClient;
            this.cardsAggregator = cardsAggregator;
        }

        public ReportModel[] BuildForBillingDelivery()
        {
            var lists = taskManagerClient.GetBoardLists(KnownBoards.BillingDelivery.Id);
            var listNames = lists.Select(l => l.Name).ToArray();
            var listNameToIdIndex = lists.ToDictionary(l => l.Name, l => l.Id);
            var aggregation = cardsAggregator.Aggregate(KnownLists.BillingDeliveryDone, KnownLists.BillingDeliveryFeedback);

            var reportsList = new List<ReportModel>
            {
                new ReportModel("full.csv", Build(aggregation, listNames, listNameToIdIndex)),
                new ReportModel("product.csv", Build(aggregation.FilterBy(KnownTaskQueuesLabels.ProductQueueLabel), listNames, listNameToIdIndex)),
                new ReportModel("crm.csv", Build(aggregation.FilterBy(KnownTaskQueuesLabels.ProductQueueLabel), listNames, listNameToIdIndex)),
                new ReportModel("infractructure.csv", Build(aggregation.FilterBy(KnownTaskQueuesLabels.InfrastructureQueueLabel), listNames, listNameToIdIndex)),
                new ReportModel("support.csv", Build(aggregation.FilterBy(KnownTaskQueuesLabels.SupportQueueLabel), listNames, listNameToIdIndex))
            };
            return reportsList.ToArray();
        }

        public ReportModel BuildForDirection(BoardList doneList)
        {
            var lists = taskManagerClient.GetBoardLists(doneList.BoardId);
            var listNames = lists.Select(l => l.Name).ToArray();
            var listNameToIdIndex = lists.ToDictionary(l => l.Name, l => l.Id);
            var aggregation = cardsAggregator.Aggregate(doneList);
            var content = Build(aggregation, listNames, listNameToIdIndex);
            return new ReportModel($"{doneList.BoardId}.csv", content);
        }

        private static byte[] Build(CardsAggregationModel cardsAggregation, string[] listNames, Dictionary<string, string> listNameToIdIndex)
        {
            var strBuilder = new StringBuilder();
            AppendReportHeader(strBuilder, listNames);
            foreach (var cardStats in cardsAggregation.CardsStats)
            {
                AppendCardStats(strBuilder, cardStats, listNames, listNameToIdIndex);
            }
            strBuilder.AppendLine(string.Empty);
            strBuilder.AppendLine("Full aggregation stats;");
            AppendAgregationStats(strBuilder, cardsAggregation.FullAggregationStats);
            strBuilder.AppendLine("S tasks aggregation stats;");
            AppendAgregationStats(strBuilder, cardsAggregation.SAggregationStats);
            strBuilder.AppendLine("M tasks aggregation stats;");
            AppendAgregationStats(strBuilder, cardsAggregation.MAggregationStats);
            strBuilder.AppendLine("L tasks aggregation stats;");
            AppendAgregationStats(strBuilder, cardsAggregation.LAggregationStats);
            strBuilder.AppendLine("XL tasks aggregation stats;");
            AppendAgregationStats(strBuilder, cardsAggregation.XLAggregationStats);
            return defaultReportEncoding.GetBytes(strBuilder.ToString());
        }

        private static void AppendAgregationStats(StringBuilder strBuilder, AggregationTimeStats cardsAggregationStats)
        {
            if (cardsAggregationStats.AreEmpty())
                return;

            strBuilder.AppendLine($"Average cycle time;{FormatTimeSpan(cardsAggregationStats.AverageTime)}");
            strBuilder.AppendLine($"Longest cycle time;{FormatTimeSpan(cardsAggregationStats.LongestTimeCard.CycleTime)}, cardName {cardsAggregationStats.LongestTimeCard.Name};");
            strBuilder.AppendLine($"Shortest cycle time;{FormatTimeSpan(cardsAggregationStats.ShortestTimeCard.CycleTime)}, cardName {cardsAggregationStats.ShortestTimeCard.Name};");
        }

        private static void AppendCardStats(StringBuilder strBuilder, CardStatsModel cardStats, string[] listNames, Dictionary<string, string> listNameToIdIndex)
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

        private static void AppendReportHeader(StringBuilder strBuilder, string[] listNames)
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