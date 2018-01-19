using IoCContainer;
using MessageBroker;
using MessageBroker.Bots;
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
            container.RegisterInstance<IYouTubeCredentialService>(credentialsService);

            var mbCredentials = credentialsService.MessageBrokerCredentials;
            var emailMessageProducer = new KonturEmailBot(container.Get<IMessenger>(),
                                                          mbCredentials.Login, mbCredentials.Password, mbCredentials.Domain, "dag3.kontur", 25);
            container.RegisterInstance<IEmailBot>(emailMessageProducer);

            var spreadsheetsMessageProducer = new GoogleSpreadsheetsBot(container.Get<IMessenger>(), credentialsService.GoogleClientSecret);
            container.RegisterInstance<ISpreadsheetsBot>(spreadsheetsMessageProducer);
        }
    }
}