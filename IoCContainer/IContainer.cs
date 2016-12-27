using System;

namespace SKBKontur.Treller.IoCContainer
{
    public interface IContainer
    {
        T Get<T>();
        object Get(Type type);
        void RegisterInstance<T>(object instance);
    }
}