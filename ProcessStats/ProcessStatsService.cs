using System.Collections.Generic;
using MessageBroker;
using ProcessStats.Dev;

namespace ProcessStats
{
    public class ProcessStatsService : IProcessStatsService
    {
        private readonly IStatsReportBuilder statsReportBuilder;
        private readonly IMessageProducer messageProducer;

        public ProcessStatsService(
            IStatsReportBuilder statsReportBuilder,
            IMessageProducer messageProducer)
        {
            this.statsReportBuilder = statsReportBuilder;
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
                Recipient = "manager.billing@skbkontur.ru",
                Body = body,
                Attachments = attachments.ToArray()
            };
            messageProducer.Publish(message);
        }

        public void BuildInfractructureStatsAndDeliverToGuild()
        {
            throw new System.NotImplementedException();
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