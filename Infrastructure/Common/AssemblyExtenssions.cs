using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SKBKontur.Infrastructure.Common
{
    public static class AssemblyExtenssions
    {
        public static IEnumerable<Assembly> ApplyDefaultFilter(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.Where(x => IsInternalAssembly(x.FullName));
        }

        public static IEnumerable<AssemblyName> ApplyDefaultFilter(this IEnumerable<AssemblyName> assemblieNames)
        {
            return assemblieNames.Where(x => IsInternalAssembly(x.FullName));
        }

        // TODO: Плохое решение, в будущем переделать, унести в Customizer
        public static bool IsInternalAssembly(this string fullAssemblyName)
        {
            return fullAssemblyName.StartsWith("SKBKontur", StringComparison.OrdinalIgnoreCase) || fullAssemblyName.Contains("Billy");
        }
    }
}