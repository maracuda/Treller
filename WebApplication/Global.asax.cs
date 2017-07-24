using System;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IoCContainer;
using Logger;
using MessageBroker;
using OperationalService;
using OperationalService.Operations;
using RepositoryHooks.BranchNotification;
using WebApplication.Implementation.Infrastructure.Credentials;
using WebApplication.Implementation.Services.News;
using WebApplication.Implementation.Services.News.NewsFeed;
using WebApplication.Implementation.VirtualMachines.Runspaces;

namespace WebApplication
{
    public class MvcApplication : HttpApplication
    {
        private IVirtualMachinesRunspacePool runspacePool;
        private IOperationalService operationalService;
        private IContainer container;

        protected void Application_Start()
        {
            PrepareWebApplication();

            container = ContainerFactory.CreateMvc();
            container.Get<ILogService>().OnError += HandleError;
            CustomizeContainer();

            runspacePool = container.Get<IVirtualMachinesRunspacePool>();
            operationalService = container.Get<IOperationalService>();
            RunRegularOperations();
        }

        private static void PrepareWebApplication()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ReactConfig.Configure();

            BundleTable.EnableOptimizations = false;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CustomizeContainer()
        {
            var credentialService = container.Get<ICredentialService>();
            var mbCredentials = credentialService.MessageBrokerCredentials;
            var emailMessageProducer = new EmailMessageProducer(mbCredentials.Login, mbCredentials.Password, mbCredentials.Domain, "dag3.kontur", 25);
            container.RegisterInstance<IMessageProducer>(emailMessageProducer);
        }

        private void HandleError(object sender, ErrorEventArgs args)
        {
            var messageBuilder = new StringBuilder(args.Message);
            if (args.Exception != null)
                messageBuilder.Append($"{Environment.NewLine}{args.Exception}{Environment.NewLine}{args.Exception.StackTrace}");
            messageBuilder.Append($"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}Occured at {Environment.MachineName}.");

            container.Get<IMessageProducer>().Publish(new Message
            {
                Recipient = "hvorost@skbkontur.ru",
                Title = args.Message,
                Body = messageBuilder.ToString()
            });
        }

        private void RunRegularOperations()
        {
            var operationsFactory = container.Get<IRegularOperationsFactory>();

            operationalService.Register(
                operationsFactory.Create("TaskManagerReporter", () => { container.Get<IBillingTimes>().LookForNews(); }),
                ScheduleParams.CreateAnytime(TimeSpan.FromMinutes(10)));
            operationalService.Register(
                operationsFactory.Create("AgingNewsActualizator", () => container.Get<INewsFeed>().Refresh()),
                ScheduleParams.CreateAnytime(TimeSpan.FromMinutes(60)));
            operationalService.Register(
                operationsFactory.Create("MergerBranchesNotificator",
                    () => container.Get<IBranchNotificator>().NotifyCommitersAboutMergedBranches(TimeSpan.FromDays(15))),
                ScheduleParams.CreateAnytime(TimeSpan.FromHours(24)));
        }

        protected void Application_End()
        {
            try
            {
                operationalService.Dispose();
                runspacePool.Dispose();
            }
            catch { }
        }
    }
}