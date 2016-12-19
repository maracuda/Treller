using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SKBKontur.Treller.IoCContainer.AssembliesLoader
{
    public class AssembliesLoader : IAssembliesLoader
    {
        private IEnumerable<Assembly> loadedAssemblies;

        public IEnumerable<Assembly> LoadAssemblies()
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