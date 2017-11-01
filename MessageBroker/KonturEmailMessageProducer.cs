using System.IO;
using System.Net;
using System.Net.Mail;

namespace MessageBroker
{
    public class KonturEmailMessageProducer : IEmailMessageProducer
    {
        private readonly string login;
        private readonly string password;
        private readonly string domain;
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string fromEmail;

        public KonturEmailMessageProducer(
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
            fromEmail = $"{login}@skbkontur.ru";
        }

        public void Publish(EmailMessage message)
        {
            if (message.Recipients == null || message.Recipients.Length < 1)
                return;

            using (var smtpClient = CreateClient())
            {
                var mailMessage = new MailMessage(fromEmail, message.Recipients[0], message.Title, message.Body);
                for (var i = 1; i < message.Recipients.Length; i++)
                {
                    mailMessage.To.Add(message.Recipients[i]);
                }
                foreach (var attachment in message.EmailAttachments)
                {
                    mailMessage.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(attachment.Content), attachment.Name));
                }
                if (!string.IsNullOrEmpty(message.ReplyTo))
                {
                    mailMessage.ReplyToList.Add(new MailAddress(message.ReplyTo));
                }
                if (!string.IsNullOrEmpty(message.CopyTo))
                {
                    mailMessage.CC.Add(new MailAddress(message.CopyTo));
                }
                smtpClient.Send(mailMessage);
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