using Logger;

namespace ProcessStats.Dev
{
    public class KanbanStats : IKanbanStats
    {
        private readonly ICardHistoryService cardHistoryService;
        private readonly ILoggerFactory loggerFactory;

        public KanbanStats(
            ICardHistoryService cardHistoryService,
            ILoggerFactory loggerFactory)
        {
            this.cardHistoryService = cardHistoryService;
            this.loggerFactory = loggerFactory;
        }

        //TODO: verify what time trello sends at client
        //TODO: unit tests
        public CardStatsModel Build(string cardId)
        {
            var cardHistory = cardHistoryService.Get(cardId);
            if (cardHistory.Movements.Length <= 1)
            {
                return new CardStatsModel
                {
                    CardId = cardId
                };
            }

            var result = new CardStatsModel
            {
                CardId = cardId
            };

            for (int i = 0; i < cardHistory.Movements.Length - 1; i++)
            {
                if (cardHistory.Movements[i].To != cardHistory.Movements[i + 1].From)
                {
                    loggerFactory.Get<KanbanStats>()
                        .LogError($"Broken actions chain obtained for cardId {cardId}. To {cardHistory.Movements[i].To}, From {cardHistory.Movements[i + 1].From}, chain index {i}.");
                }
                else
                {
                    var duration = cardHistory.Movements[i + 1].Date.Subtract(cardHistory.Movements[i].Date);
                    switch (cardHistory.Movements[i].To)
                    {
                        case DevelopingProcessStage.Analyzing:
                            result.AnaliticsDuration += duration;
                            break;
                        case DevelopingProcessStage.Developing:
                            result.DevDuration += duration;
                            break;
                        case DevelopingProcessStage.Reviewing:
                            result.ReviewDuration += duration;
                            break;
                        case DevelopingProcessStage.Testing:
                            result.TestingDuration += duration;
                            break;
                    }
                }
            }

            return result;
        }
    }
}