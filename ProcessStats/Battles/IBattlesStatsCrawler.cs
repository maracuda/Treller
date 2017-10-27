using System;

namespace ProcessStats.Battles
{
    public interface IBattlesStatsCrawler
    {
        BattlesStats Collect(DateTime date);
    }
}