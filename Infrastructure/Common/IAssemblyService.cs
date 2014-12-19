using System;
using System.Collections.Generic;
using System.Reflection;

namespace SKBKontur.Infrastructure.Common
{
    public interface IAssemblyService
    {
        IEnumerable<Assembly> GetLoadedAssemblies();
        IEnumerable<Assembly> GetAssemblies(Func<Assembly, bool> predicate);
        IEnumerable<Assembly> GetAssembliesFromLoaded(Func<Assembly, bool> predicate);
        Type[] GetAllDerivedTypes(Type baseType);
    }
}