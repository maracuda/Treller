using LightInject;
using SKBKontur.Treller.IoCContainer.AssembliesLoader;
using SKBKontur.Treller.IoCContainer.Implementation;

namespace SKBKontur.Treller.IoCContainer
{
    public class ContainerConfigurator
    {
        public IContainer Configure()
        {
            var containterImpl = new ServiceContainer();

            var assembliesLoader = new AssembliesLoader.AssembliesLoader();
            foreach (var assembly in assembliesLoader.LoadAssemblies())
            {
                containterImpl.RegisterAssembly(assembly, () => new PerContainerLifetime());
            }
            containterImpl.Register<IServiceContainer>(factory => containterImpl, new PerContainerLifetime());;
            containterImpl.Register<IAssembliesLoader>(factory => assembliesLoader, new PerContainerLifetime());;
            var result = new LightInjectContainer(containterImpl);
            containterImpl.Register<IContainer>(factory => result, new PerContainerLifetime());

            return result;
        }
    }
}