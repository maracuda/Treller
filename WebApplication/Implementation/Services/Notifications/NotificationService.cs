using System;
using System.Net;
using System.Net.Mail;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IAdService adService;
        private const string SenderEmail = "maylo@skbkontur.ru";
        private const string AnalitycsEmail = "ask.billing@skbkontur.ru";

        public NotificationService(IAdService adService)
        {
            this.adService = adService;
        }

        public void SendErrorReport(string errorHeader, Exception exception)
        {
            var body = string.Format("{0}{1}{2}", errorHeader, Environment.NewLine, exception);

            using (var smtpClient = CreateClientWithCredentials())//new SmtpClient("smtp.kontur", 25) { UseDefaultCredentials = true, DeliveryMethod = SmtpDeliveryMethod.Network })
            {
                smtpClient.Send(new MailMessage(SenderEmail, SenderEmail, errorHeader, body));
            }
        }

        public void SendMessage(string recipientEmail, string messageHeader, string messageBody, bool inHtmlStyle)
        {
            using (var smtpClient = CreateClientWithCredentials())
            {
                smtpClient.Send(new MailMessage(SenderEmail, recipientEmail, messageHeader, messageBody) { IsBodyHtml = inHtmlStyle, ReplyToList = { AnalitycsEmail } });
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