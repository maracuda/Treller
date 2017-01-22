using System;
using LightInject;

namespace SKBKontur.Treller.IoCContainer.Implementation
{
    public class LightInjectContainer : IContainer
    {
        private readonly ServiceContainer serviceContainer;

        public LightInjectContainer(ServiceContainer serviceContainer)
        {
            this.serviceContainer = serviceContainer;
        }

        public T Get<T>()
        {
            return serviceContainer.GetInstance<T>();
        }

        public object Get(Type type)
        {
            return serviceContainer.GetInstance(type);
        }

        public void RegisterInstance<T>(object instance)
        {
            serviceContainer.RegisterInstance(typeof(T), instance);
        }

        public T Create<T>() where T : class
        {
            return serviceContainer.Create<T>();
        }
    }
}