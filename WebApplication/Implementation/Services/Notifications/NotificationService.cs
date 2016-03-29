using System;
using System.Net;
using System.Net.Mail;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IAdService adService;
        private const string AnalitycsEmail = "ask.billing@skbkontur.ru";
        private readonly string notificationReciever;
        private readonly string senderEmail;

        public NotificationService(IAdService adService, INotificationCredentials notificationCredentials)
        {
            this.adService = adService;
            senderEmail = string.Format("{0}@skbkontur.ru", adService.GetDeliveryCredentials().Login);
            notificationReciever = notificationCredentials.GetNotificationEmailAddress();
        }

        public void SendErrorReport(string errorHeader, Exception exception)
        {
            var body = string.Format("{0}{1}{2}", errorHeader, Environment.NewLine, exception);

            using (var smtpClient = CreateClientWithCredentials())
            {
                smtpClient.Send(new MailMessage(senderEmail, notificationReciever, errorHeader, body));
            }
        }

        public void SendMessage(string recipientEmail, string messageHeader, string messageBody, bool inHtmlStyle)
        {
            using (var smtpClient = CreateClientWithCredentials())
            {
                smtpClient.Send(new MailMessage(senderEmail, recipientEmail, messageHeader, messageBody) { IsBodyHtml = inHtmlStyle, ReplyToList = { AnalitycsEmail } });
            }
        }

        private SmtpClient CreateClientWithCredentials()
        {
            var credentials = adService.GetDeliveryCredentials();
            return new SmtpClient("dag3.kontur", 25)
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(credentials.Login, credentials.Password, credentials.Domain)
            };
        }
    }
}