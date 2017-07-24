using IoCContainer;
using Storage;
using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Repository.Clients.GitLab;
using Tests.Tests.IntegrationTests.Configuration;

namespace Tests.Tests.IntegrationTests
{
    public abstract class IntegrationTest
    {
        protected readonly IContainer container;

        protected IntegrationTest()
        {
            container = ContainerFactory.Create();
            container.RegisterInstance<IEnvironment>(new TestEnvironment());
            var credentialsService = container.Create<CredentialService>();
            container.RegisterInstance<ITrelloUserCredentialService>(credentialsService);
            container.RegisterInstance<IGitLabCredentialService>(credentialsService);
        }
    }
}