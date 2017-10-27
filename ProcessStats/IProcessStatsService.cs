namespace ProcessStats
{
    public interface IProcessStatsService
    {
        void BuildAllAndDeliverToManagers();
        void BuildInfractructureStatsAndDeliverToGuild();
        void CollectAndPublishBattlesStats();
        void CollectAndPublishIncidentsStats();
    }
}