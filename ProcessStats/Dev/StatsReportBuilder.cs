using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerClient;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public class StatsReportBuilder : IStatsReportBuilder
    {
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
                new ReportModel("full", Build(aggregation, listNames, listNameToIdIndex)),
                new ReportModel("product", Build(aggregation.FilterBy(KnownTaskQueuesLabels.ProductQueueLabel), listNames, listNameToIdIndex)),
                new ReportModel("crm", Build(aggregation.FilterBy(KnownTaskQueuesLabels.CrmQueueLabel), listNames, listNameToIdIndex)),
                new ReportModel("infractructure", Build(aggregation.FilterBy(KnownTaskQueuesLabels.InfrastructureQueueLabel), listNames, listNameToIdIndex)),
                new ReportModel("support", Build(aggregation.FilterBy(KnownTaskQueuesLabels.SupportQueueLabel), listNames, listNameToIdIndex))
            };
            return reportsList.ToArray();
        }

        public ReportModel BuildForDirection(BoardList doneList, BoardList additionalDoneList = null)
        {
            var lists = taskManagerClient.GetBoardLists(doneList.BoardId);
            var listNames = lists.Select(l => l.Name).ToArray();
            var listNameToIdIndex = lists.ToDictionary(l => l.Name, l => l.Id);
            var aggregation = cardsAggregator.Aggregate(doneList, additionalDoneList);
            var content = Build(aggregation, listNames, listNameToIdIndex);
            return new ReportModel(doneList.BoardId, content);
        }

        private static IList<ReportRow> Build(CardsAggregationModel cardsAggregation, string[] listNames, Dictionary<string, string> listNameToIdIndex)
        {
            var rowsList = new List<ReportRow> {BuildHeader(listNames)};
            foreach (var cardStats in cardsAggregation.CardsStats)
            {
                rowsList.Add(BuildCardStats(cardStats, listNames, listNameToIdIndex));
            }
            rowsList.Add(ReportRow.Empty);
            rowsList.AddRange(BuildAgregationStats("Full aggregation stats", cardsAggregation.FullAggregationStats));
            rowsList.AddRange(BuildAgregationStats("S tasks aggregation stats", cardsAggregation.SAggregationStats));
            rowsList.AddRange(BuildAgregationStats("M tasks aggregation stats", cardsAggregation.MAggregationStats));
            rowsList.AddRange(BuildAgregationStats("L tasks aggregation stats", cardsAggregation.LAggregationStats));
            rowsList.AddRange(BuildAgregationStats("XL tasks aggregation stats", cardsAggregation.XLAggregationStats));

            return rowsList;
        }

        private static ReportRow[] BuildAgregationStats(string header, AggregationTimeStats cardsAggregationStats)
        {
            if (cardsAggregationStats.AreEmpty())
                return new ReportRow[0];

            return new[]
            {
                ReportRow.Create(header),
                ReportRow.Create("Average cycle time", FormatTimeSpan(cardsAggregationStats.AverageTime)),
                ReportRow.Create("Longest cycle time", FormatTimeSpan(cardsAggregationStats.LongestTimeCard.CycleTime), $"cardName {cardsAggregationStats.LongestTimeCard.Name}"), 
                ReportRow.Create("Shortest cycle time", FormatTimeSpan(cardsAggregationStats.ShortestTimeCard.CycleTime), $"cardName {cardsAggregationStats.ShortestTimeCard.Name}"), 
            };
        }

        private static ReportRow BuildCardStats(CardStatsModel cardStats, string[] listNames, Dictionary<string, string> listNameToIdIndex)
        {
            var reportRow = ReportRow.Create(cardStats.Name, cardStats.Size, FormatTimeSpan(cardStats.CycleTime));
            foreach (var listName in listNames)
            {
                var listId = listNameToIdIndex[listName];
                if (cardStats.ListStats.ContainsKey(listId))
                {
                    reportRow.Append(FormatTimeSpan(cardStats.ListStats[listId]));
                }
            }
            return reportRow;
        }

        private static ReportRow BuildHeader(string[] listNames)
        {
            var reportRow = ReportRow.Create("Name", "Size", "CycleTime");
            foreach (var listName in listNames)
            {
                reportRow.Append(listName);
            }
            return reportRow;
        }

        private static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Days},{Math.Round((double)timeSpan.Hours / 24 * 10).ToString().First()}";
        }
    }
}