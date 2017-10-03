namespace ProcessStats
{
    public interface IProcessStatsService
    {
        void BuildAllAndDeliverToManagers();
        void BuildInfractructureStatsAndDeliverToGuild();
    }
}