using System;
using System.Collections.Generic;
using System.Linq;
using LightInject;

namespace SKBKontur.Treller.IoCContainer.Implementation
{
    public class LightInjectContainer : IContainer
    {
        private readonly ServiceContainer _serviceContainer;

        public LightInjectContainer(ServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        public T Get<T>()
        {
            return GetSingle(GetAll<T>());
        }

        public T[] GetAll<T>()
        {
            try
            {
                return _serviceContainer.GetAllInstances<T>().ToArray();
            }
            catch (Exception ex)
            {
                throw new ContainerGetException(typeof(T), ex);
            }
        }

        public object Get(Type type)
        {
            return GetSingle(GetAll(type).ToArray());
        }

        private static T GetSingle<T>(T[] instances)
        {
            if (instances.Length != 1)
            {
                throw new ContainerGetException(typeof (T));
            }
            return instances[0];
        }

        public IEnumerable<object> GetAll(Type type)
        {
            return _serviceContainer.GetAllInstances(type);
        }

        public void RegisterInstance<T>(object instance)
        {
            _serviceContainer.RegisterInstance(typeof(T), instance);
        }

        public void RegisterInstance2<T, TImpl>(TImpl implementation) where TImpl : T
        {
            _serviceContainer.Register<T>(factory => implementation, new PerContainerLifetime());
        }
    }
}