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

        public CardsAggregationStatsModel BuildDetalization(string boardId, string doneListId, string additionalDoneListId = null)
        {
            var cardIds = taskManagerClient.GetBoardLists(boardId).Where(x => x.Id.Equals(doneListId) || (additionalDoneListId != null && x.Id.Equals(additionalDoneListId)))
                                           .Select(l => l.Cards.Select(c => c.Id))
                                           .SelectMany(i => i)
                                           .ToArray();
            return CardsAggregationStatsModel.Create(cardIds.Select(cardId => cardStatsBuilder.Build(cardId)).ToArray());
        }
    }
}