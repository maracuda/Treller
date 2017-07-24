namespace ProcessStats.Dev
{
    public interface IKanbanStats
    {
        CardStatsModel Build(string cardId);
    }
}
