using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SKBKontur.Infrastructure.Common
{
    public class AssemblyService : IAssemblyService
    {
        private IEnumerable<Assembly> loadedAssemblies;
        private readonly ConcurrentDictionary<Type, Type[]> derivedTypes;

        public AssemblyService()
        {
            derivedTypes = new ConcurrentDictionary<Type, Type[]>();
        }

        public IEnumerable<Assembly> GetLoadedAssemblies()
        {
            if (loadedAssemblies != null)
            {
                return loadedAssemblies;
            }

            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies().ApplyDefaultFilter().ToArray();
            loadedAssemblies = currentAssemblies.Union(currentAssemblies.SelectMany(x => x.GetReferencedAssemblies().ApplyDefaultFilter()).Select(Assembly.Load));
            return loadedAssemblies;
        }

        public IEnumerable<Assembly> GetAssemblies(Func<Assembly, bool> predicate)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(predicate);
        }

        public IEnumerable<Assembly> GetAssembliesFromLoaded(Func<Assembly, bool> predicate)
        {
            return loadedAssemblies.Where(predicate);
        }

        public Type[] GetAllDerivedTypes(Type baseType)
        {
            return derivedTypes.GetOrAdd(baseType,
                                     type => GetLoadedAssemblies()
                                            .SelectMany(a => a.GetTypes().Where(x => x.IsSubclassOf(type) && !x.IsAbstract))
                                            .Distinct()
                                            .ToArray());
        }
    }
}