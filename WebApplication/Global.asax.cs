using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LightInject;
using Newtonsoft.Json;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Infrastructure.ContainerConfiguration;
using SKBKontur.Treller.TrelloClient;
using SKBKontur.Treller.WebApplication.Services.Abstractions;
using System.Linq;

namespace SKBKontur.Treller.WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = new ContainerConfigurator(new TrelloClientCustomizer()).Configure();
            var serviceContainer = container.Get<IServiceContainer>();
            var assemblyService = container.Get<IAssemblyService>();

            serviceContainer.RegisterControllers(assemblyService.GetLoadedAssemblies().ToArray());
            serviceContainer.EnableMvc();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleTable.EnableOptimizations = false;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            

            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new ActionConverter());
                return settings;
            });
        }
    }
}