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
        private readonly IEmailMessageProducer emailMessageProducer;

        public ProcessStatsService(
            IStatsReportBuilder statsReportBuilder,
            IBattlesStatsCrawler battlesStatsCrawler,
            IIncidentsStatsCrawler incidentsStatsCrawler,
            ISpreadsheetsMessageProducer spreadsheetsMessageProducer,
            IEmailMessageProducer emailMessageProducer)
        {
            this.statsReportBuilder = statsReportBuilder;
            this.battlesStatsCrawler = battlesStatsCrawler;
            this.incidentsStatsCrawler = incidentsStatsCrawler;
            this.spreadsheetsMessageProducer = spreadsheetsMessageProducer;
            this.emailMessageProducer = emailMessageProducer;
        }
        public void BuildAllAndDeliverToManagers()
        {
            var reportsList = new List<ReportModel>();
            reportsList.AddRange(statsReportBuilder.BuildForBillingDelivery());
            reportsList.AddRange(statsReportBuilder.BuildForDirections());

            foreach (var reportModel in reportsList)
            {
                spreadsheetsMessageProducer.Rewrite("1HxfCoYYQsyevahb1qnHqjTgxVvA_zw_a8nM0ijE5Bm0", reportModel.Name, reportModel.Rows.Select(r => DataRow.Create(r.Values)).ToArray());
            }

            emailMessageProducer.Publish(new EmailMessage
            {
                Recipients = new[] { "hvorost@skbkontur.ru" },
                Title = "Отчет о сборе статистики",
                Body = "Дорогой менеджер, роботы успешно обновили статистику работы команды"
            });
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

            emailMessageProducer.Publish(new EmailMessage
            {
                Recipients = new[] { "hvorost@skbkontur.ru" },
                Title = "Отчет о сборе батлов",
                Body = $"Дорогой менеджер, роботы успешно обновили статистику по баттлам: {date.Value:dd.MM.yyyy}, открыто {battlesStats.CreatedCount}, переоткрыто {battlesStats.ReopenCount}, исправлено {battlesStats.FixedCount}."
            });
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

            emailMessageProducer.Publish(new EmailMessage
            {
                Recipients = new[] { "hvorost@skbkontur.ru" },
                Title = "Отчет о сборе инцидентов",
                Body = $"Дорогой менеджер, роботы успешно обновили статистику по инцидентам: {date.Value:dd.MM.yyyy}, входящие {incidentsStats.IncomingCount}, исправлено {incidentsStats.FixedCount}."
            });
        }
    }
}