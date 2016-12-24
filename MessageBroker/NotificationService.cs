using System.Net;
using System.Net.Mail;

namespace SKBKontur.Treller.MessageBroker
{
    public class NotificationService : INotificationService
    {
        private readonly string login;
        private readonly string password;
        private readonly string domain;
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string senderEmail;

        public NotificationService(
            string login,
            string password,
            string domain,
            string smtpHost,
            int smtpPort)
        {
            this.login = login;
            this.password = password;
            this.domain = domain;
            this.smtpHost = smtpHost;
            this.smtpPort = smtpPort;
            senderEmail = $"{login}@skbkontur.ru";
        }

        public void Send(Notification notification)
        {
            if (string.IsNullOrEmpty(notification.Recipient))
                return;

            using (var smtpClient = CreateClient())
            {
                var message = new MailMessage(senderEmail, notification.Recipient, notification.Title, notification.Body);

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
            return new SmtpClient(smtpHost, smtpPort)
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(login, password, domain)
            };
        }
    }
}