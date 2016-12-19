using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;
using SKBKontur.Treller.IoCContainer;
using SKBKontur.Treller.Tests.Tests.IntegrationTests.Configuration;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests
{
    public abstract class IntegrationTest
    {
        protected readonly IContainer container;

        protected IntegrationTest()
        {
            container = ContainerFactory.Create();
            var credentialsService = new CredentialService();
            container.RegisterInstance<ITrelloUserCredentialService>(credentialsService);
            container.RegisterInstance<IGitLabCredentialService>(credentialsService);
        }
    }
}