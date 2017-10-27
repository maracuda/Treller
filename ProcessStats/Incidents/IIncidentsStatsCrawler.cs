using System;

namespace ProcessStats.Incidents
{
    public interface IIncidentsStatsCrawler
    {
        IncidentsStats Collect(DateTime date);
    }
}