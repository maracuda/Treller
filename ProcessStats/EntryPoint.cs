using System;
using IoCContainer;
using MessageBroker;
using ProcessStats.Config;
using TaskManagerClient.CredentialServiceAbstractions;

namespace ProcessStats
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"started at {AppDomain.CurrentDomain.BaseDirectory}.");

            var container = ContainerFactory.Create();
            var credentialsService = container.Create<CredentialService>();
            container.RegisterInstance<ITrelloUserCredentialService>(credentialsService);

            var mbCredentials = credentialsService.MessageBrokerCredentials;
            var emailMessageProducer = new EmailMessageProducer(mbCredentials.Login, mbCredentials.Password, mbCredentials.Domain, "dag3.kontur", 25);
            container.RegisterInstance<IMessageProducer>(emailMessageProducer);

            container.Get<IProcessStatsService>().BuildAllAndDeliverToManagers();

            Console.WriteLine("done");
        }
    }
}