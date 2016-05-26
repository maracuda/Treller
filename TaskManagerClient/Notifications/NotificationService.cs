using System.Net;
using System.Net.Mail;

namespace SKBKontur.TaskManagerClient.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationCredentialsService notificationCredentialsService;
        private readonly string senderEmail;

        public NotificationService(INotificationCredentialsService notificationCredentialsService)
        {
            this.notificationCredentialsService = notificationCredentialsService;
            senderEmail = $"{notificationCredentialsService.GetNotificationCredentials().Login}@skbkontur.ru";
        }

        public void Send(Notification notification)
        {
            if (string.IsNullOrEmpty(notification.Recipient))
                return;

            using (var smtpClient = CreateClient())
            {
                var message = new MailMessage(senderEmail, notification.Recipient, notification.Title, notification.Body)
                {
                    IsBodyHtml = notification.IsHtml,
                };
                if (!string.IsNullOrEmpty(notification.ReplyTo))
                {
                    message.ReplyToList.Add(new MailAddress(notification.ReplyTo));
                }
                if (!string.IsNullOrEmpty(notification.CopyTo))
                {
                    message.CC.Add(new MailAddress(notification.CopyTo));
                }
                smtpClient.Send(message);
            }
        }

        private SmtpClient CreateClient()
        {
            var credentials = notificationCredentialsService.GetNotificationCredentials();
            return new SmtpClient("dag3.kontur", 25)
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(credentials.Login, credentials.Password, credentials.Domain)
            };
        }
    }
}