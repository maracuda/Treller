using System;
using SKBKontur.Treller.MessageBroker;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService
{
    public class ErrorService : IErrorService
    {
        private const string NotificationFileName = "NotificationEmail";

        private readonly IMessageProducer messageProducer;
        private readonly IKeyValueStorage keyValueStorage;

        public ErrorService(
            IMessageProducer messageProducer,
            IKeyValueStorage keyValueStorage)
        {
            this.messageProducer = messageProducer;
            this.keyValueStorage = keyValueStorage;

            ErrorRecipientEmail = keyValueStorage.Find<string>(NotificationFileName) ?? "hvorost@skbkontur.ru";
        }

        public string ErrorRecipientEmail { get; private set; }
        public void SendError(string title, Exception ex)
        {
            SendError(title, $"{title}{Environment.NewLine}{ex}");
        }

        public void SendError(string title)
        {
            SendError(title, title);
        }

        private void SendError(string title, string body)
        {
            var notification = new Message
            {
                Title = title,
                Body = body,
                Recipient = ErrorRecipientEmail,
            };
            messageProducer.Publish(notification);
        }

        public void ChangeErrorRecipientEmail(string email)
        {
            ErrorRecipientEmail = email;
            keyValueStorage.Write(NotificationFileName, email);
        }
    }
}