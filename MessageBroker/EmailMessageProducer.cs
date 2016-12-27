using System.Net;
using System.Net.Mail;

namespace SKBKontur.Treller.MessageBroker
{
    public class EmailMessageProducer : IMessageProducer
    {
        private readonly string login;
        private readonly string password;
        private readonly string domain;
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string senderEmail;

        public EmailMessageProducer(
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

        public void Publish(Message message)
        {
            if (string.IsNullOrEmpty(message.Recipient))
                return;

            using (var smtpClient = CreateClient())
            {
                var mailMessage = new MailMessage(senderEmail, message.Recipient, message.Title, message.Body);

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