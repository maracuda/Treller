using NUnit.Framework;
using SKBKontur.Infrastructure.ContainerConfiguration;

namespace SKBKontur.TestInfrastructure.Tests.IntegrationTests
{
    [TestFixture]
    public abstract class IntegrationTest
    {
        protected IContainer container;

        [SetUp]
        public virtual void SetUp()
        {
            var configurator = new ContainerConfigurator();
            container = configurator.Configure();
        }

        [TearDown]
        public virtual void TearDown()
        {
        }
    }
}