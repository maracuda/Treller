using System.Net;
using System.Net.Mail;
using MessageBroker.Messages;

namespace MessageBroker.Bots
{
    public class KonturEmailBot : IEmailBot
    {
        private readonly Member me;
        private readonly string login;
        private readonly string password;
        private readonly string domain;
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string fromEmail;

        public KonturEmailBot(
            IMessenger messenger,
            string login,
            string password,
            string domain,
            string smtpHost,
            int smtpPort)
        {
            messenger.ChatRegistred += OnChatRegistred;
            me = messenger.RegisterBotMember(GetType());
            this.login = login;
            this.password = password;
            this.domain = domain;
            this.smtpHost = smtpHost;
            this.smtpPort = smtpPort;
            fromEmail = $"{login}@skbkontur.ru";
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

        private void OnChatRegistred(object sender, NewChatEventArgs eventArgs)
        {
            eventArgs.Chat.NewMessagePosted += FilterEmailAndHandle;
        }

        private void FilterEmailAndHandle(object sender, MessageEventArgs eventArgs)
        {
            if (eventArgs.Message is Email email)
            {
                Publish(email);
                if (sender is IChat chat)
                {
                    chat.Post(me, $"Опубликовано: {email}.");
                }
            }
        }

        public void Publish(Email message)
        {
            if (message.Recipients == null || message.Recipients.Length < 1)
                return;

            using (var smtpClient = CreateClient())
            {
                var mailMessage = new MailMessage(fromEmail, message.Recipients[0], message.Title, message.Body)
                {
                    IsBodyHtml = true
                };
                for (var i = 1; i < message.Recipients.Length; i++)
                {
                    mailMessage.To.Add(message.Recipients[i]);
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

        public void Dispose()
        {
        }
    }
}