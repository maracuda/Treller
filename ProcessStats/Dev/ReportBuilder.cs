using System.Linq;
using TaskManagerClient;

namespace ProcessStats.Dev
{
    public class ReportBuilder : IReportBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ICardStatsBuilder cardStatsBuilder;

        public ReportBuilder(
            ITaskManagerClient taskManagerClient,
            ICardStatsBuilder cardStatsBuilder)
        {
            this.taskManagerClient = taskManagerClient;
            this.cardStatsBuilder = cardStatsBuilder;
        }

        public CardsAggregationStatsModel BuildDetalization(string boardId)
        {
            const string waitForFeedbackListId = "58d2295834d4e5619d71d008";
            const string doneListId = "58d2296edeed2d3c3e25acdd";
            var cardIds = taskManagerClient.GetBoardLists(boardId).Where(x => x.Id.Equals(waitForFeedbackListId) || x.Id.Equals(doneListId))
                                           .Select(l => l.Cards.Select(c => c.Id))
                                           .SelectMany(i => i)
                                           .ToArray();
            return CardsAggregationStatsModel.Create(cardIds.Select(cardId => cardStatsBuilder.Build(cardId)).ToArray());
        }
    }
}