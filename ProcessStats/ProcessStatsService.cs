using System;
using System.Collections.Generic;
using System.Linq;
using MessageBroker;
using ProcessStats.Battles;
using ProcessStats.Dev;
using ProcessStats.Incidents;

namespace ProcessStats
{
    public class ProcessStatsService : IProcessStatsService
    {
        private readonly IStatsReportBuilder statsReportBuilder;
        private readonly IBattlesStatsCrawler battlesStatsCrawler;
        private readonly IIncidentsStatsCrawler incidentsStatsCrawler;
        private readonly ISpreadsheetsMessageProducer spreadsheetsMessageProducer;

        public ProcessStatsService(
            IStatsReportBuilder statsReportBuilder,
            IBattlesStatsCrawler battlesStatsCrawler,
            IIncidentsStatsCrawler incidentsStatsCrawler,
            ISpreadsheetsMessageProducer spreadsheetsMessageProducer)
        {
            this.statsReportBuilder = statsReportBuilder;
            this.battlesStatsCrawler = battlesStatsCrawler;
            this.incidentsStatsCrawler = incidentsStatsCrawler;
            this.spreadsheetsMessageProducer = spreadsheetsMessageProducer;
        }
        public void BuildAllAndDeliverToManagers()
        {
            var reportsList = new List<ReportModel>();
            reportsList.AddRange(statsReportBuilder.BuildForBillingDelivery());
            reportsList.Add(statsReportBuilder.BuildForDirection(KnownLists.MotocycleDone));
            reportsList.Add(statsReportBuilder.BuildForDirection(KnownLists.PortalAuthDone));
            reportsList.Add(statsReportBuilder.BuildForDirection(KnownLists.MarketDone));
            reportsList.Add(statsReportBuilder.BuildForDirection(KnownLists.DiscountsDone));

            foreach (var reportModel in reportsList)
            {
                spreadsheetsMessageProducer.Rewrite("1HxfCoYYQsyevahb1qnHqjTgxVvA_zw_a8nM0ijE5Bm0", reportModel.Name, reportModel.Rows.Select(r => DataRow.Create(r.Values)).ToArray());
            }
        }

        public void BuildInfractructureStatsAndDeliverToGuild()
        {
            throw new System.NotImplementedException();
        }

        public void CollectAndPublishBattlesStats(DateTime? date = null)
        {
            if (!date.HasValue)
            {
                date = DateTime.Now.AddDays(-1).Date;
            }
            var battlesStats = battlesStatsCrawler.Collect(date.Value);
            var dataRow = DataRow.Create(battlesStats.Date, battlesStats.CreatedCount, battlesStats.ReopenCount, battlesStats.FixedCount);
            spreadsheetsMessageProducer.Append("1FVrVCLPDiXgWwq2nGOabeMlT27Muxtm3_OTZQn82SAE", "Батлы", dataRow);
        }

        public void CollectAndPublishIncidentsStats(DateTime? date = null)
        {
            if (!date.HasValue)
            {
                date = DateTime.Now.AddDays(-1).Date;
            }
            var incidentsStats = incidentsStatsCrawler.Collect(date.Value);
            var dataRow = DataRow.Create(incidentsStats.Date, incidentsStats.IncomingCount, incidentsStats.FixedCount);
            spreadsheetsMessageProducer.Append("1FVrVCLPDiXgWwq2nGOabeMlT27Muxtm3_OTZQn82SAE", "Инциденты", dataRow);
        }
    }
}