using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using LightInject;

namespace SKBKontur.Infrastructure.ContainerConfiguration
{
    public class ConainerImplementedScope : IDependencyScope
    {
        private readonly Scope _scope;
        private readonly IContainer _container;

        public ConainerImplementedScope(Scope scope, IContainer container)
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