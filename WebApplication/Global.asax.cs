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
using WebApplication.Implementation.Infrastructure.Credentials;
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
                Recipients = new []{ "hvorost@skbkontur.ru" },
                Title = args.Message,
                Body = messageBuilder.ToString()
            });
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