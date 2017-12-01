using System;
using System.Reflection;
using ConsoleRunner.Config;
using IoCContainer;
using MessageBroker;
using TaskManagerClient.CredentialServiceAbstractions;
using ProcessStats;
using RepositoryHooks.BranchNotification;

namespace ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"started at {AppDomain.CurrentDomain.BaseDirectory}.");
            if (args.Length != 3)
            {
                Console.WriteLine("Not enough parameters. The first is service lib name, the second is service type, the third is void method name to run.");
                return;
            }

            var container = ConfigureContainer();

            Type serviceType;
            object serviceInstance;
            try
            {
                serviceType = Type.GetType($"{args[1]}, {args[0]}");
                Console.WriteLine($"Service type parsed as {serviceType}.");
                serviceInstance = container.Get<IProcessStatsService>();
                serviceInstance = container.Get<IBranchNotificator>();
                serviceInstance = container.Get(serviceType);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fail to parse service type from string {args[1]}, {args[0]}. InnerException: {e}.");
                return;
            }

            MethodInfo method;
            try
            {
                method = serviceType.GetMethod(args[2]);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fail to parse service method from string {args[2]}. InnerException: {e}.");
                return;
            }

            Console.WriteLine($"begin method {method.Name} invocation.");
            var parametersCount = method.GetParameters().Length;
            var parameters = new object[parametersCount];
            method.Invoke(serviceInstance, parameters);
            Console.WriteLine("end");
        }

        private static IContainer ConfigureContainer()
        {
            var container = ContainerFactory.Create();
            var credentialsService = container.Create<CredentialService>();
            container.RegisterInstance<ITrelloUserCredentialService>(credentialsService);
            container.RegisterInstance<IYouTrackCredentialService>(credentialsService);

            var mbCredentials = credentialsService.MessageBrokerCredentials;
            var emailMessageProducer = new KonturEmailMessageProducer(mbCredentials.Login, mbCredentials.Password, mbCredentials.Domain, "dag3.kontur", 25);
            container.RegisterInstance<IEmailMessageProducer>(emailMessageProducer);

            var spreadsheetsMessageProducer = new GoogleSpreadsheetsMessageProducer(credentialsService.ClientSecret);
            container.RegisterInstance<ISpreadsheetsMessageProducer>(spreadsheetsMessageProducer);

            return container;
        }
    }
}
