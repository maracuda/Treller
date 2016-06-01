using System;
using System.Collections.Generic;

namespace SKBKontur.Infrastructure.ContainerConfiguration
{
    public interface IContainer
    {
        T Get<T>();
        T[] GetAll<T>();
        object Get(Type type);
        IEnumerable<object> GetAll(Type type);
        void RegisterInstance<T>(object instance);
    }
}