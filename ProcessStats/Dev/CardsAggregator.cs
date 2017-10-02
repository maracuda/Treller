using System;
using System.Linq;
using TaskManagerClient;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public class CardsAggregator : ICardsAggregator
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ICardStatsBuilder cardStatsBuilder;

        public CardsAggregator(
            ITaskManagerClient taskManagerClient,
            ICardStatsBuilder cardStatsBuilder)
        {
            this.taskManagerClient = taskManagerClient;
            this.cardStatsBuilder = cardStatsBuilder;
        }

        public CardsAggregationModel Aggregate(BoardList doneList, BoardList additionalDoneList = null)
        {
            if (additionalDoneList != null && !string.Equals(doneList.BoardId, additionalDoneList.BoardId))
                throw new ArgumentException($"Both lists should be from single board! Observed boards ids: {doneList.BoardId}, {additionalDoneList.BoardId}.");

            var additionalDoneListId = additionalDoneList?.Id;
            var cardIds = taskManagerClient.GetBoardLists(doneList.BoardId).Where(x => x.Id.Equals(doneList.Id) || (additionalDoneListId != null && x.Id.Equals(additionalDoneListId)))
                .Select(l => l.Cards.Select(c => c.Id))
                .SelectMany(i => i)
                .ToArray();
            return CardsAggregationModel.Create(cardIds.Select(cardId => cardStatsBuilder.Build(cardId)).ToArray());
        }
    }
}