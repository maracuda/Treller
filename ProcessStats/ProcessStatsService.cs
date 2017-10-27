﻿using System;
using System.Collections.Generic;
using MessageBroker;
using ProcessStats.Battles;
using ProcessStats.Dev;
using ProcessStats.Incidents;
using ProcessStats.SpreadsheetProducer;

namespace ProcessStats
{
    public class ProcessStatsService : IProcessStatsService
    {
        private readonly IStatsReportBuilder statsReportBuilder;
        private readonly IBattlesStatsCrawler battlesStatsCrawler;
        private readonly IIncidentsStatsCrawler incidentsStatsCrawler;
        private readonly ISpreadsheetProducer spreadsheetProducer;
        private readonly IMessageProducer messageProducer;

        public ProcessStatsService(
            IStatsReportBuilder statsReportBuilder,
            IBattlesStatsCrawler battlesStatsCrawler,
            IIncidentsStatsCrawler incidentsStatsCrawler,
            ISpreadsheetProducer spreadsheetProducer,
            IMessageProducer messageProducer)
        {
            this.statsReportBuilder = statsReportBuilder;
            this.battlesStatsCrawler = battlesStatsCrawler;
            this.incidentsStatsCrawler = incidentsStatsCrawler;
            this.spreadsheetProducer = spreadsheetProducer;
            this.messageProducer = messageProducer;
        }
        public void BuildAllAndDeliverToManagers()
        {
            var attachments = new List<Attachment>();
            AppendAsAttachment(attachments, statsReportBuilder.BuildForBillingDelivery());
            AppendAsAttachment(attachments, statsReportBuilder.BuildForDirection(KnownLists.MotocycleDone));
            AppendAsAttachment(attachments, statsReportBuilder.BuildForDirection(KnownLists.PortalAuthDone));
            AppendAsAttachment(attachments, statsReportBuilder.BuildForDirection(KnownLists.MarketDone));
            AppendAsAttachment(attachments, statsReportBuilder.BuildForDirection(KnownLists.DiscountsDone));

            const string body = "Дорогой менеджер!\r\n\r\n" +
                                "Спешу сообщить тебе актуальную статистику по работе команды.\r\n" +
                                "Пожалуйста, посмотри как ей можно воспользоваться чтобы улучшить работу команды.\r\n\r\n" +
                                "С любовью, твой автоматический уведомлятор.\r\n";
            var message = new Message
            {
                Title = "Статистика работы команды Биллинга",
                Recipients = new []{ "manager.billing@skbkontur.ru", "nesterenko@skbkontur.ru" },
                Body = body,
                Attachments = attachments.ToArray()
            };
            messageProducer.Publish(message);
        }

        public void BuildInfractructureStatsAndDeliverToGuild()
        {
            throw new System.NotImplementedException();
        }

        public void CollectAndPublishBattlesStats()
        {
            var battlesStats = battlesStatsCrawler.Collect(DateTime.Now.AddDays(-1).Date);
            spreadsheetProducer.Publish("1FVrVCLPDiXgWwq2nGOabeMlT27Muxtm3_OTZQn82SAE", 724378477, "Батлы", new[] { battlesStats.Date.ToString("yyyy-MM-dd"), battlesStats.CreatedCount.ToString(), battlesStats.ReopenCount.ToString(), battlesStats.FixedCount.ToString() });
        }

        public void CollectAndPublishIncidentsStats()
        {
            var incidentsStats = incidentsStatsCrawler.Collect(DateTime.Now.Date);
            spreadsheetProducer.Publish("1FVrVCLPDiXgWwq2nGOabeMlT27Muxtm3_OTZQn82SAE", 0, "Инциденты", new[] { incidentsStats.Date.ToString("yyyy-MM-dd"), incidentsStats.IncomingCount.ToString(), incidentsStats.FixedCount.ToString() });
        }

        private static void AppendAsAttachment(List<Attachment> attachments, ReportModel[] reportModels)
        {
            foreach (var reportModel in reportModels)
            {
                AppendAsAttachment(attachments, reportModel);
            }
        }

        private static void AppendAsAttachment(List<Attachment> attachments, ReportModel reportModel)
        {
            attachments.Add(new Attachment
            {
                Name = reportModel.Name,
                Content = reportModel.Content
            });
        }
    }
}