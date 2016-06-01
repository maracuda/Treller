using System;
using System.Collections.Generic;
using System.Linq;
using LightInject;

namespace SKBKontur.Infrastructure.ContainerConfiguration
{
    public class Container : IContainer
    {
        private readonly ServiceContainer _serviceContainer;

        public Container(ServiceContainer serviceContainer)
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
    }
}