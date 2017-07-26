using System.Linq;
using TaskManagerClient;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public class CardHistoryService : ICardHistoryService
    {
        private readonly ITaskManagerClient taskManagerClient;

        public CardHistoryService(ITaskManagerClient taskManagerClient)
        {
            this.taskManagerClient = taskManagerClient;
        }

        public CardHistory Get(string cardId)
        {
            var updateActions = taskManagerClient.GetCardUpdateActions(cardId);
            var movements = updateActions.Where(x => x.ToList != null && x.FromList != null)
                .Select(BuildMovement)
                .OrderBy(x => x.Date)
                .ToArray();
            var createAction = updateActions.FirstOrDefault(a => a.Type == ActionType.CreateCard);

            return new CardHistory
            {
                Movements = movements,
                CreateDate = createAction?.Date
            };
        }

        private static CardMovement BuildMovement(CardAction action)
        {
            return CardMovement.Create(action.FromList.Id, action.FromList.Name, action.ToList.Id, action.ToList.Name, action.Date);
        }
    }
}