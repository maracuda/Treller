using System.Linq;
using LightInject;
using SKBKontur.Treller.IoCContainer.AssembliesLoader;
using SKBKontur.Treller.IoCContainer.Implementation;

namespace SKBKontur.Treller.IoCContainer
{
    public static class ContainerFactory
    {
        public static IContainer Create()
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

        public static IContainer CreateMvc()
        {
            var containterImpl = new ServiceContainer();

            var assembliesLoader = new AssembliesLoader.AssembliesLoader();
            var assemblies = assembliesLoader.LoadAssemblies().ToArray();
            foreach (var assembly in assemblies)
            {
                containterImpl.RegisterAssembly(assembly, () => new PerContainerLifetime());
            }
            containterImpl.Register<IServiceContainer>(factory => containterImpl, new PerContainerLifetime()); ;
            containterImpl.Register<IAssembliesLoader>(factory => assembliesLoader, new PerContainerLifetime()); ;
            var result = new LightInjectContainer(containterImpl);
            containterImpl.Register<IContainer>(factory => result, new PerContainerLifetime());
            containterImpl.RegisterControllers(assemblies);
            containterImpl.EnableMvc();

            return result;
        }
    }
}