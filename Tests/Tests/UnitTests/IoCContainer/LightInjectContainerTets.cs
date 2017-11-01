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
            var emailMessageProducer = new KonturEmailMessageProducer("login", "password", "kontur", "localhost", 25);
            container.RegisterInstance<IEmailMessageProducer>(emailMessageProducer);

            var result = container.Get<IEmailMessageProducer>();
            Assert.NotNull(result);
        }
    }
}
