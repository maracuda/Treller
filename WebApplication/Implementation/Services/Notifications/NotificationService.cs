using System.Net;
using System.Net.Mail;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IAdService adService;
        private readonly string senderEmail;

        public NotificationService(IAdService adService)
        {
            this.adService = adService;
            senderEmail = $"{adService.GetDeliveryCredentials().Login}@skbkontur.ru";
        }

        public void SendMessage(string recipientEmail, string messageTitle, string messageBody, bool inHtmlStyle, string replyTo = null)
        {
            //TODO: handle cerafully this case
            if (string.IsNullOrEmpty(recipientEmail))
                return;

            using (var smtpClient = CreateClient())
            {
                var message = new MailMessage(senderEmail, recipientEmail, messageTitle, messageBody)
                {
                    IsBodyHtml = inHtmlStyle,
                };
                if (!string.IsNullOrEmpty(replyTo))
                {
                    message.ReplyToList.Add(replyTo);
                }
                smtpClient.Send(message);
            }
        }

        private SmtpClient CreateClient()
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