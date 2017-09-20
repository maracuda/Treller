using System;
using Logger;
using TaskManagerClient;

namespace ProcessStats.Dev
{
    public class CardStatsBuilder : ICardStatsBuilder
    {
        private readonly ICardHistoryService cardHistoryService;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ILoggerFactory loggerFactory;

        public CardStatsBuilder(
            ICardHistoryService cardHistoryService,
            ITaskManagerClient taskManagerClient,
            ILoggerFactory loggerFactory)
        {
            this.cardHistoryService = cardHistoryService;
            this.taskManagerClient = taskManagerClient;
            this.loggerFactory = loggerFactory;
        }

        //TODO: verify what time trello sends at client
        //TODO: unit tests
        public CardStatsModel Build(string cardId)
        {
            var cardHistory = cardHistoryService.Get(cardId);
            var card = taskManagerClient.GetCard(cardId);
            var result = new CardStatsModel
            {
                Id = cardId,
                Name = card.Name
            };

            result.SetLabels(card.Labels);

            if (cardHistory.Movements.Length <= 1)
            {
                return result;
            }

            if (cardHistory.CreateDate.HasValue)
            {
                AddDuration(result, cardHistory.Movements[0].FromListId, cardHistory.Movements[0].Date.Subtract(cardHistory.CreateDate.Value));
            }
            for (var i = 0; i < cardHistory.Movements.Length - 1; i++)
            {
                if (!string.Equals(cardHistory.Movements[i].ToListId, cardHistory.Movements[i + 1].FromListId))
                {
                    loggerFactory.Get<CardStatsBuilder>()
                        .LogError($"Broken actions chain obtained for cardId {cardId}. To {cardHistory.Movements[i].ToListId}, From {cardHistory.Movements[i + 1].FromListId}, chain index {i}.");
                }
                else
                {
                    var listId = cardHistory.Movements[i + 1].FromListId;
                    var duration = cardHistory.Movements[i + 1].Date.Subtract(cardHistory.Movements[i].Date);
                    AddDuration(result, listId, duration);
                }
            }
            return result;
        }

        //TODO: move func to model
        private static void AddDuration(CardStatsModel cardStatsModel, string listId, TimeSpan duration)
        {
            if (cardStatsModel.ListStats.ContainsKey(listId))
            {
                cardStatsModel.ListStats[listId] = cardStatsModel.ListStats[listId] + duration;
            }
            else
            {
                cardStatsModel.ListStats.Add(listId, duration);
            }
        }
    }
}