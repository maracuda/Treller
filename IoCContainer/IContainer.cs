using System;

namespace IoCContainer
{
    public interface IContainer
    {
        T Get<T>();
        object Get(Type type);
        T Create<T>() where T : class;
        void RegisterInstance<T>(object instance);
    }
}