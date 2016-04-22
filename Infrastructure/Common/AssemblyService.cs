using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SKBKontur.Infrastructure.Common
{
    public class AssemblyService : IAssemblyService
    {
        private IEnumerable<Assembly> loadedAssemblies;

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
    }
}