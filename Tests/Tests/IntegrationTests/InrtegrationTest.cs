using NUnit.Framework;
using SKBKontur.Infrastructure.ContainerConfiguration;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;
using SKBKontur.Treller.Tests.Tests.IntegrationTests.Configuration;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests
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
            var credentialsService = new CredentialService();
            container.RegisterInstance<ITrelloUserCredentialService>(credentialsService);
            container.RegisterInstance<IGitLabCredentialService>(credentialsService);
        }

        [TearDown]
        public virtual void TearDown()
        {
        }
    }
}