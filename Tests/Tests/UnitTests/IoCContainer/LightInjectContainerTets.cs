using IoCContainer;
using MessageBroker;
using MessageBroker.Bots;
using Xunit;

namespace Tests.Tests.UnitTests.IoCContainer
{
    public class LightInjectContainerTets : UnitTest
    {
        [Fact]
        public void CanRegisterCustomImplementationToContainer()
        {
            var container = ContainerFactory.CreateMvc();
            var emailMessageProducer = new KonturEmailBot(container.Get<IMessenger>(), 
                                                                      "login", "password", "kontur", "localhost", 25);
            container.RegisterInstance<IEmailBot>(emailMessageProducer);

            var result = container.Get<IEmailBot>();
            Assert.NotNull(result);
        }
    }
}
