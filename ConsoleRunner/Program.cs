using System;
using System.Collections.Generic;
using System.Reflection;
using ConsoleRunner.Config;
using IoCContainer;
using MessageBroker;
using MessageBroker.Bots;
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

            var container = CreateContainer();
            var armyOfBots = CreateArmyOfBots(container);
            //NOTE: add this starange code to fail fast bacause I got problems with container at execution stage. 
            container.Get<IProcessStatsService>();
            container.Get<IBranchNotificator>();

            Type serviceType;
            object serviceInstance;
            try
            {
                serviceType = Type.GetType($"{args[1]}, {args[0]}");
                Console.WriteLine($"Service type parsed as {serviceType}.");
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

            DestroyArmyOfBots(armyOfBots);

            Console.WriteLine("end");
        }

        private static IContainer CreateContainer()
        {
            var container = ContainerFactory.Create();
            var credentialsService = container.Create<CredentialService>();
            container.RegisterInstance<ITrelloUserCredentialService>(credentialsService);
            container.RegisterInstance<IYouTrackCredentialService>(credentialsService);
            return container;
        }

        private static IEnumerable<IBot> CreateArmyOfBots(IContainer container)
        {
            var credentialsService = container.Create<CredentialService>();
            var armyOfBots = new List<IBot>();

            var mbCredentials = credentialsService.MessageBrokerCredentials;
            var emailBot = new KonturEmailBot(container.Get<IMessenger>(),
                mbCredentials.Login, mbCredentials.Password, mbCredentials.Domain, "dag3.kontur", 25);
            container.RegisterInstance<IEmailBot>(emailBot);
            armyOfBots.Add(emailBot);

            var spreadsheetsBot = new GoogleSpreadsheetsBot(container.Get<IMessenger>(), credentialsService.GoogleClientSecret);
            container.RegisterInstance<ISpreadsheetsBot>(spreadsheetsBot);
            armyOfBots.Add(spreadsheetsBot);
            return armyOfBots;
        }

        private static void DestroyArmyOfBots(IEnumerable<IBot> armyOfBots)
        {
            foreach (var armyOfBot in armyOfBots)
            {
                armyOfBot.Dispose();
            }
        }
    }
}
