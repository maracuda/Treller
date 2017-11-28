using System;

namespace ProcessStats
{
    public interface IProcessStatsService
    {
        void BuildAllAndDeliverToManagers();
        void BuildInfractructureStatsAndDeliverToGuild();
        void CollectAndPublishBattlesStats(DateTime? date = null);
        void CollectAndPublishIncidentsStats(DateTime? date = null);
    }
}