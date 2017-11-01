using System;
using System.Collections.Generic;
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
            var attachments = new List<EmailAttachment>();
            AppendAsAttachment(attachments, statsReportBuilder.BuildForBillingDelivery());
            AppendAsAttachment(attachments, statsReportBuilder.BuildForDirection(KnownLists.MotocycleDone));
            AppendAsAttachment(attachments, statsReportBuilder.BuildForDirection(KnownLists.PortalAuthDone));
            AppendAsAttachment(attachments, statsReportBuilder.BuildForDirection(KnownLists.MarketDone));
            AppendAsAttachment(attachments, statsReportBuilder.BuildForDirection(KnownLists.DiscountsDone));

            const string body = "Дорогой менеджер!\r\n\r\n" +
                                "Спешу сообщить тебе актуальную статистику по работе команды.\r\n" +
                                "Пожалуйста, посмотри как ей можно воспользоваться чтобы улучшить работу команды.\r\n\r\n" +
                                "С любовью, твой автоматический уведомлятор.\r\n";
            var message = new EmailMessage
            {
                Title = "Статистика работы команды Биллинга",
                Recipients = new []{ "manager.billing@skbkontur.ru", "nesterenko@skbkontur.ru" },
                Body = body,
                EmailAttachments = attachments.ToArray()
            };
            emailMessageProducer.Publish(message);
        }

        public void BuildInfractructureStatsAndDeliverToGuild()
        {
            throw new System.NotImplementedException();
        }

        public void CollectAndPublishBattlesStats()
        {
            var battlesStats = battlesStatsCrawler.Collect(DateTime.Now.AddDays(-1).Date);
            spreadsheetsMessageProducer.Publish("1FVrVCLPDiXgWwq2nGOabeMlT27Muxtm3_OTZQn82SAE", 724378477, new object[] { battlesStats.Date, battlesStats.CreatedCount, battlesStats.ReopenCount, battlesStats.FixedCount });
        }

        public void CollectAndPublishIncidentsStats()
        {
            var incidentsStats = incidentsStatsCrawler.Collect(DateTime.Now.AddDays(-1).Date);
            spreadsheetsMessageProducer.Publish("1FVrVCLPDiXgWwq2nGOabeMlT27Muxtm3_OTZQn82SAE", 0, new object[] { incidentsStats.Date, incidentsStats.IncomingCount, incidentsStats.FixedCount });
        }

        private static void AppendAsAttachment(List<EmailAttachment> attachments, ReportModel[] reportModels)
        {
            foreach (var reportModel in reportModels)
            {
                AppendAsAttachment(attachments, reportModel);
            }
        }

        private static void AppendAsAttachment(List<EmailAttachment> attachments, ReportModel reportModel)
        {
            attachments.Add(new EmailAttachment
            {
                Name = reportModel.Name,
                Content = reportModel.Content
            });
        }
    }
}