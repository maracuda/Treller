using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public interface ICardsAggregator
    {
        CardsAggregationModel Aggregate(BoardList doneList, BoardList additionalDoneList = null);
    }
}