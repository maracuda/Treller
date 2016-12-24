using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web;
using SKBKontur.Treller.IoCContainer;
using SKBKontur.Treller.Logger;
using SKBKontur.Treller.MessageBroker;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials;
using SKBKontur.Treller.WebApplication.Implementation.Repository;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Migration;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Scheduler;
using SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.Runspaces;

namespace SKBKontur.Treller.WebApplication
{
    public class MvcApplication : HttpApplication
    {
        private IVirtualMachinesRunspacePool runspacePool;
        private IOperationalService operationalService;
        private IContainer container;

        protected void Application_Start()
        {
            container = ContainerFactory.CreateMvc();
            container.Get<ILogService>().OnError += HandleError;
            CustomizeContainer();
            PrepareWebApplication();

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

        private void HandleError(object sender, ErrorEventArgs args)
        {
            container.Get<IMessageProducer>().Publish(new Message
            {
                Recipient = "hvorost@skbkontur.ru",
                Title = args.Message,
                Body = args.Exception == null
                    ? $"{args.Message}{Environment.NewLine}{args.Exception}"
                    : args.Message
            });
        }

        private void CustomizeContainer()
        {
            var credentialService = container.Get<ICredentialService>();
            var mbCredentials = credentialService.MessageBrokerCredentials;
            var notificationService = new EmailMessageProducer(mbCredentials.Login, mbCredentials.Password, mbCredentials.Domain, "dag3.kontur", 25);
            container.RegisterInstance<IMessageProducer>(notificationService);
        }

        private void RunRegularOperations()
        {
            container.Get<NullReportConsistencyChecker>().Run();

            var operationsFactory = container.Get<IRegularOperationsFactory>();

            operationalService.Register(
                operationsFactory.Create("TaskManagerReporter", () => { container.Get<IBillingTimes>().LookForNews(); }),
                ScheduleParams.CreateAnytime(TimeSpan.FromMinutes(10)));
            operationalService.Register(
                operationsFactory.Create("AgingNewsActualizator", () => container.Get<INewsFeed>().Refresh()),
                ScheduleParams.CreateAnytime(TimeSpan.FromMinutes(60)));
            operationalService.Register(
                operationsFactory.Create("MergerBranchesNotificator",
                    () => container.Get<IRepositoryNotificator>().NotifyCommitersAboutMergedBranches(TimeSpan.FromDays(15))),
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