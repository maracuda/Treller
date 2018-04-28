namespace ProcessStats.Battles
{
    public interface IBattlesStatsCrawler
    {
        BattlesStats Collect();
    }
}