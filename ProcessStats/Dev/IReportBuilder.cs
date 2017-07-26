namespace ProcessStats.Dev
{
    public interface IReportBuilder
    {
        CardsAggregationStatsModel BuildDetalization(string boardId);
    }
}