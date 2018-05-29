using MessageBroker;
using MessageBroker.Bots;
using MessageBroker.Messages;
using Xunit;

namespace Tests.Tests.IntegrationTests.MessageBroker
{
    public class EmailBotTest : IntegrationTest
    {
        private readonly IEmailBot emailBot;

        public EmailBotTest()
        {
            emailBot = container.Get<IEmailBot>();
        }

        [Fact]
        public void TestMessageWithSeveralRecipients()
        {
            var message = new Email
            {
                Title = "test message",
                Recipients = new []{ "hvorost@skbkontur.ru", "jaamal@mail.ru" },
                Body = "test content <b>sdslksdklsdkl</b> <a href=\"https://google.com\">{branchName}</a>"
            };
            emailBot.Publish(message);
        }

        [Fact]
        public void TestPublishMessageFromNewChat()
        {
            var message = new Email
            {
                Title = "test message",
                Recipients = new[] { "hvorost@skbkontur.ru" },
                Body = "test content"
            };

            var messenger = container.Get<IMessenger>();
            var testChat = messenger.RegisterChat("test chat", string.Empty);
            testChat.Post(messenger.RegisterBotMember(GetType()), message);
        }
    }
}