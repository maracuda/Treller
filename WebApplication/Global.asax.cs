using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LightInject;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Infrastructure.ContainerConfiguration;
using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Consitency;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Import;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskCacher;
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

            operationalService.Register(operationsFactory.Create("NewsRefresher", TimeSpan.FromMinutes(5), () => container.Get<INewsService>().Refresh()));
            operationalService.Register(operationsFactory.Create("NewsImporter", TimeSpan.FromMinutes(10), () => container.Get<INewsImporter>().ImportAll()));
            operationalService.Register(operationsFactory.Create("EveningNewsPublisher", TimeSpan.FromMinutes(5), TimeSpan.Parse("18:20:00"), TimeSpan.Parse("10:00:00"), () => container.Get<INewsService>().SendNews()));
            operationalService.Register(operationsFactory.Create("AfterNoonNewsPublisher", TimeSpan.FromMinutes(5), TimeSpan.Parse("12:00:00"), TimeSpan.Parse("13:00:00"), () => container.Get<INewsService>().SendNews()));
            operationalService.Register(operationsFactory.Create("NewsConsistencyInspector", TimeSpan.FromMinutes(10), () => container.Get<IConsistencyIspector>().Run()));

            var cacheActualizerFunc = new Func<long, long>(timestamp => container.Get<ITaskCacher>().Actualize(new DateTime(timestamp)).Ticks);
            operationalService.Register(operationsFactory.Create("CacheActualizer", TimeSpan.FromMinutes(1), cacheActualizerFunc, () => DateTime.UtcNow.AddDays(-2).Ticks));
            //NOTE: turn off this process since it always fails (staff need personal domain account to send messages)
            //operationalService.Register("DigestSender", TimeSpan.FromMinutes(5), () => container.Get<IDigestService>().SendAllToDigest());
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