using System.Linq;
using System.Web.Http;
using LightInject;
using Owin;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Infrastructure.ContainerConfiguration;

namespace SKBKontur.WebApiInfrastructure.WebApi
{
    public class WebApiConfigurator
    {
        public static IContainer Container;

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            Container = new ContainerConfigurator().Configure();

            config.Routes.MapHttpRoute(
                name: "default",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            var serviceContainer = Container.Get<IServiceContainer>();
            var assemblyService = Container.Get<IAssemblyService>();
            serviceContainer.EnableWebApi(config);
            serviceContainer.RegisterApiControllers(assemblyService.GetLoadedAssemblies().ToArray());
            
            appBuilder.UseWebApi(config);
            
        }
    }
}