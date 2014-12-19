using System.Web.Http;
using SKBKontur.Treller.TrelloClient;

namespace SKBKontur.Treller.WebApplication.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new ActionConverter());
        }
    }
}
