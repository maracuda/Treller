using SKBKontur.Treller.MessageBroker;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher
{
    public class NewsNotificator : INewsNotificator
    {
        private readonly IMessageProducer messageProducer;

        public NewsNotificator(IMessageProducer messageProducer)
        {
            this.messageProducer = messageProducer;
        }

        public void NotifyAboutReleases(string mailingList, string title, string text)
        {
            var body = $"Всем доброго времени суток.\r\n\r\n{text}\r\n\r\nВы можете ответить на это письмо, если у вас возникли вопросы или комментарии касающиеся релизов\r\n\r\n--\r\nС уважением, команда Контур.Биллинг";
            var notification = new Message
            {
                Title = title,
                Body = body,
                Recipient = mailingList,
                ReplyTo = "ask.billing@skbkontur.ru"
            };
            messageProducer.Publish(notification);
        }
    }
}