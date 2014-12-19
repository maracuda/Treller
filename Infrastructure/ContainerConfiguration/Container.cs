using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
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
//            if (typeof(T) == typeof(object))
//            {
//                return default(T);
//            }

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

        public void RegisterType<T>()
        {
        }

        public IDependencyScope GetScope()
        {
            return new MyScope(_serviceContainer.BeginScope(), this);
        }
    }

    public class MyScope : IDependencyScope
    {
        private readonly Scope _scope;
        private readonly IContainer _container;

        public MyScope(Scope scope, IContainer container)
        {
            _scope = scope;
            _container = container;
        }

        public void Dispose()
        {
            _scope.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return _container.Get(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAll(serviceType);
        }
    }
}