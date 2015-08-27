using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LightInject;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Infrastructure.ContainerConfiguration;
using SKBKontur.Treller.WebApplication.App_Start;
using System.Linq;
using SKBKontur.Treller.WebApplication.Services.TaskCacher;

namespace SKBKontur.Treller.WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
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
            container.Get<IOperationalService>().Start();
        }
    }
}