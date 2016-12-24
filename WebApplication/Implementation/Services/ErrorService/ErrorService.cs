using System;
using SKBKontur.Treller.MessageBroker;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService
{
    public class ErrorService : IErrorService
    {
        private readonly IMessageProducer messageProducer;

        public ErrorService(
            IMessageProducer messageProducer)
        {
            this.messageProducer = messageProducer;
        }

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
                Recipient = "hvorost@skbkontur.ru",
            };
            messageProducer.Publish(notification);
        }
    }
}