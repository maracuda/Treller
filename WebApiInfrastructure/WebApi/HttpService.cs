using System;
using Microsoft.Owin.Hosting;

namespace SKBKontur.WebApiInfrastructure.WebApi
{
    public static class HttpService
    {
         public static void StartConsole(string serviceUrl, string serviceName)
         {
             using (WebApp.Start<WebApiConfigurator>(serviceUrl))
             {
                 Console.WriteLine("Hello I`m {0} console service", serviceName);
                 Console.WriteLine("Started on {0}", serviceUrl);

                 var preStartRunnerses = WebApiConfigurator.Container.GetAll<IPreStartRunner>();
                 foreach (var startRunner in preStartRunnerses)
                 {
                     startRunner.Run();
                 }

                 Console.WriteLine("Stop me by entering any key...");
                 Console.ReadLine();
             }
         }
    }
}