using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LightInject;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Infrastructure.ContainerConfiguration;
using System.Linq;
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
    public class MvcApplication : System.Web.HttpApplication
    {
        private IVirtualMachinesRunspacePool runspacePool;
        private IOperationalService operationalService;

        protected void Application_Start()
        {
            var container = new ContainerConfigurator().Configure();
            var serviceContainer = container.Get<IServiceContainer>();
            var assemblyService = container.Get<IAssemblyService>();

            serviceContainer.RegisterControllers(assemblyService.GetLoadedAssemblies().ToArray());
            serviceContainer.EnableMvc();
            
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ReactConfig.Configure();

            BundleTable.EnableOptimizations = false;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            runspacePool = container.Get<IVirtualMachinesRunspacePool>();

            var operationsFactory = container.Get<IRegularOperationsFactory>();
            operationalService = container.Get<IOperationalService>();

            container.Get<NullReportConsistencyChecker>().Run();

            operationalService.Register(operationsFactory.Create("TaskManagerReporter", () => { container.Get<IBillingTimes>().LookForNews(); }), ScheduleParams.CreateAnytime(TimeSpan.FromMinutes(10)));
            operationalService.Register(operationsFactory.Create("AgingNewsActualizator", () => container.Get<INewsFeed>().Refresh()), ScheduleParams.CreateAnytime(TimeSpan.FromMinutes(60)));
            operationalService.Register(operationsFactory.Create("MergerBranchesNotificator", () => container.Get<IRepositoryNotificator>().NotifyCommitersAboutMergedBranches(TimeSpan.FromDays(15))), ScheduleParams.CreateAnytime(TimeSpan.FromHours(24)));
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