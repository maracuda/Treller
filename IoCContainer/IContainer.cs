using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.IoCContainer
{
    public interface IContainer
    {
        T Get<T>();
        T[] GetAll<T>();
        object Get(Type type);
        IEnumerable<object> GetAll(Type type);
        void RegisterInstance<T>(object instance);
        void RegisterInstance2<T, TImpl>(TImpl implementation) where TImpl : T;
    }
}