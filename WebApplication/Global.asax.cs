using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LightInject;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Infrastructure.ContainerConfiguration;
using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Services.Digest;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals;
using SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.Runspaces;

namespace SKBKontur.Treller.WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IVirtualMachinesRunspacePool runspacePool;
        private IOperationalService operationalService;
        private IOperationalService2 operationalService2;

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
            operationalService = container.Get<IOperationalService>();
            operationalService.Start();

            operationalService2 = container.Get<IOperationalService2>();
            operationalService2.RegisterRegularProccess("NewsRefresher", () => container.Get<INewsService>().Refresh(), TimeSpan.FromMinutes(5));
            //NOTE: turn off this process since it always fails (staff need personal domain account to send messages)
            //operationalService2.RegisterRegularProccess("DigestSender", () => container.Get<IDigestService>().SendAllToDigest(), TimeSpan.FromMinutes(5));
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