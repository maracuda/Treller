using IoCContainer;
using MessageBroker;
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
            container.RegisterInstance<IYouTrackCredentialService>(credentialsService);
            container.RegisterInstance<ISpreadsheetsCredentialService>(credentialsService);
            container.RegisterInstance<IGoogleApiCredentialService>(credentialsService);

            var mbCredentials = credentialsService.MessageBrokerCredentials;
            var emailMessageProducer = new KonturEmailMessageProducer(mbCredentials.Login, mbCredentials.Password, mbCredentials.Domain, "dag3.kontur", 25);
            container.RegisterInstance<IEmailMessageProducer>(emailMessageProducer);

            var spreadsheetsMessageProducer = new GoogleSpreadsheetsMessageProducer(credentialsService.GoogleClientSecret);
            container.RegisterInstance<ISpreadsheetsMessageProducer>(spreadsheetsMessageProducer);
        }
    }
}