namespace ProcessStats.Dev
{
    public interface ICardStatsBuilder
    {
        CardStatsModel Build(string cardId);
    }
}
