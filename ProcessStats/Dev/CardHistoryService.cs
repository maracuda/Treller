using System.Linq;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SkbKontur.Treller.ProcessStats.Dev
{
    public class CardHistoryService : ICardHistoryService
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly IDevelopingProcessStageParser developingProcessStageParser;

        public CardHistoryService(
            ITaskManagerClient taskManagerClient,
            IDevelopingProcessStageParser developingProcessStageParser)
        {
            this.taskManagerClient = taskManagerClient;
            this.developingProcessStageParser = developingProcessStageParser;
        }

        public CardHistory Get(string cardId)
        {
            var updateActions = taskManagerClient.GetCardUpdateActions(cardId);
            var movements = updateActions.Where(x => x.ToList != null && x.FromList != null)
                .Select(BuildMovement)
                .OrderBy(x => x.Date)
                .ToArray();

            return new CardHistory
            {
                Movements = movements
            };
        }

        private CardMovement BuildMovement(CardAction action)
        {
            var from = developingProcessStageParser.TryParse(action.FromList.Name);
            var to = developingProcessStageParser.TryParse(action.ToList.Name);
            return CardMovement.Create(from, to, action.Date);
        }
    }
}