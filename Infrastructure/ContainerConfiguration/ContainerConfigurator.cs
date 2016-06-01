using LightInject;
using SKBKontur.Infrastructure.Common;

namespace SKBKontur.Infrastructure.ContainerConfiguration
{
    public class ContainerConfigurator
    {
        public IContainer Configure()
        {
            var serviceContainer = new ServiceContainer();

            var assemblyService = new AssemblyService();
            foreach (var assembly in assemblyService.GetLoadedAssemblies())
            {
                serviceContainer.RegisterAssembly(assembly, () => new PerContainerLifetime());
            }
            serviceContainer.Register<IServiceContainer>(factory => serviceContainer, new PerContainerLifetime());;
            serviceContainer.Register<IAssemblyService>(factory => assemblyService, new PerContainerLifetime());;
            var result = new Container(serviceContainer);
            serviceContainer.Register<IContainer>(factory => result, new PerContainerLifetime());

            return result;
        }
    }
}