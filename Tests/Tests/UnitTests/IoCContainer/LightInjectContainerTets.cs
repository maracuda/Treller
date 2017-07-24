using IoCContainer;
using MessageBroker;
using Xunit;

namespace Tests.Tests.UnitTests.IoCContainer
{
    public class LightInjectContainerTets : UnitTest
    {
        [Fact]
        public void CanRegisterCustomImplementationToContainer()
        {
            var container = ContainerFactory.CreateMvc();
            var emailMessageProducer = new EmailMessageProducer("login", "password", "kontur", "localhost", 25);
            container.RegisterInstance<IMessageProducer>(emailMessageProducer);

            var result = container.Get<IMessageProducer>();
            Assert.NotNull(result);
        }
    }
}
