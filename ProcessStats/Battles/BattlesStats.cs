namespace ProcessStats.Battles
{
    public class BattlesStats
    {
        public int UnassignedCount { get; set; }
        public SubProductBattleStats[] SubProducStats { get; set; }
        public SubProductFuckupStats[] SubProductsStats { get; set; }
    }
}