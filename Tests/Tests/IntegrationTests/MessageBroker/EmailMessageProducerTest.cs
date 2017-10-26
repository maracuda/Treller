using MessageBroker;
using Xunit;

namespace Tests.Tests.IntegrationTests.MessageBroker
{
    public class EmailMessageProducerTest : IntegrationTest
    {
        private IMessageProducer messageProducer;

        public EmailMessageProducerTest()
        {
            messageProducer = container.Get<IMessageProducer>();
        }

        [Fact]
        public void TestMessageWithSeveralRecipients()
        {
            var message = new Message
            {
                Title = "test message",
                Recipients = new []{ "hvorost@skbkontur.ru", "jaamal@mail.ru" },
                Body = "test content"
            };
            messageProducer.Publish(message);
        }
    }
}