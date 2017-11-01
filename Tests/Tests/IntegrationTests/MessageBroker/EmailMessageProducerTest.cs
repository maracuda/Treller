using MessageBroker;
using Xunit;

namespace Tests.Tests.IntegrationTests.MessageBroker
{
    public class EmailMessageProducerTest : IntegrationTest
    {
        private IEmailMessageProducer emailMessageProducer;

        public EmailMessageProducerTest()
        {
            emailMessageProducer = container.Get<IEmailMessageProducer>();
        }

        [Fact]
        public void TestMessageWithSeveralRecipients()
        {
            var message = new EmailMessage
            {
                Title = "test message",
                Recipients = new []{ "hvorost@skbkontur.ru", "jaamal@mail.ru" },
                Body = "test content"
            };
            emailMessageProducer.Publish(message);
        }
    }
}