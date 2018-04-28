using System;
using System.Collections.Generic;
using System.Linq;
using MessageBroker;
using MessageBroker.Messages;
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
        private readonly IChat processStatsChat;
        private readonly Member me;

        public ProcessStatsService(
            IStatsReportBuilder statsReportBuilder,
            IBattlesStatsCrawler battlesStatsCrawler,
            IIncidentsStatsCrawler incidentsStatsCrawler,
            IMessenger messenger)
        {
            this.statsReportBuilder = statsReportBuilder;
            this.battlesStatsCrawler = battlesStatsCrawler;
            this.incidentsStatsCrawler = incidentsStatsCrawler;
            processStatsChat = messenger.RegisterChat("Уведовления о сборе статистики", string.Empty);
            me = messenger.RegisterBotMember(GetType());
        }
        public void BuildAllAndDeliverToManagers()
        {
            var reportsList = new List<ReportModel>();
            reportsList.AddRange(statsReportBuilder.BuildForBillingDelivery());
            reportsList.AddRange(statsReportBuilder.BuildForDirections());

            foreach (var reportModel in reportsList)
            {
                processStatsChat.Post(me, new Report
                {
                    SpreadsheetId = "1HxfCoYYQsyevahb1qnHqjTgxVvA_zw_a8nM0ijE5Bm0",
                    SheetName = reportModel.Name,
                    Type = ReportType.Full,
                    DataRows = reportModel.Rows.Select(r => DataRow.Create(r.Values)).ToArray()
                });
            }

            processStatsChat.Post(me, new Email
            {
                Recipients = new[] { "hvorost@skbkontur.ru" },
                Title = "Отчет о сборе статистики",
                Body = "Дорогой менеджер, роботы успешно обновили статистику работы команды"
            });
        }

        public void CollectAndPublishBattlesStats(DateTime? date = null)
        {
            var battlesStats = battlesStatsCrawler.Collect();
            processStatsChat.Post(me, new Metric { Name = "Battles.Unassigned", Value = battlesStats.UnassignedCount});
            processStatsChat.Post(me, new Email
            {
                Recipients = new[] { "hvorost@skbkontur.ru" },
                Title = "Отчет о сборе батлов",
                Body = $"{battlesStats.UnassignedCount}"
            });
        }

        public void CollectAndPublishIncidentsStats(DateTime? date = null)
        {
            if (!date.HasValue)
            {
                date = DateTime.Now.AddDays(-1).Date;
            }
            var incidentsStats = incidentsStatsCrawler.Collect(date.Value);
            processStatsChat.Post(me, new Metric { Name = "Incidents.Incoming", Value = incidentsStats.IncomingCount});
            processStatsChat.Post(me, new Metric { Name = "Incidents.Fixed", Value = incidentsStats.FixedCount});

            processStatsChat.Post(me, new Email
            {
                Recipients = new[] { "hvorost@skbkontur.ru" },
                Title = "Отчет о сборе инцидентов",
                Body = $"Дорогой менеджер, роботы успешно обновили статистику по инцидентам: {date.Value:dd.MM.yyyy}, входящие {incidentsStats.IncomingCount}, исправлено {incidentsStats.FixedCount}."
            });
        }
    }
}