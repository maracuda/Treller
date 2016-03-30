using System;
using System.Net;
using System.Net.Mail;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IAdService adService;
        private readonly ICachedFileStorage cachedFileStorage;
        private const string AnalitycsEmail = "ask.billing@skbkontur.ru";
        private const string DefaultNoficationRecipientEmail = "maylo@skbkontur.ru";
        private const string NotificationFileName = "NotificationEmail";
        private string notificationRecipientEmail;
        private readonly string senderEmail;

        public NotificationService(IAdService adService, ICachedFileStorage cachedFileStorage)
        {
            this.adService = adService;
            this.cachedFileStorage = cachedFileStorage;
            senderEmail = string.Format("{0}@skbkontur.ru", adService.GetDeliveryCredentials().Login);
        }

        public void SendErrorReport(string errorHeader, Exception exception)
        {
            var body = string.Format("{0}{1}{2}", errorHeader, Environment.NewLine, exception);

            using (var smtpClient = CreateClientWithCredentials())
            {
                smtpClient.Send(new MailMessage(senderEmail, GetNotificationRecipient(), errorHeader, body));
            }
        }

        public void SendMessage(string recipientEmail, string messageHeader, string messageBody, bool inHtmlStyle)
        {
            using (var smtpClient = CreateClientWithCredentials())
            {
                smtpClient.Send(new MailMessage(senderEmail, recipientEmail, messageHeader, messageBody) { IsBodyHtml = inHtmlStyle, ReplyToList = { AnalitycsEmail } });
            }
        }

        public void ChangeNotificationRecipient(string newEmail)
        {
            notificationRecipientEmail = newEmail;
            cachedFileStorage.Write(NotificationFileName, newEmail);
        }

        public string GetNotificationRecipient()
        {
            notificationRecipientEmail = notificationRecipientEmail ?? cachedFileStorage.Find<string>(NotificationFileName) ?? DefaultNoficationRecipientEmail;
            return notificationRecipientEmail;
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