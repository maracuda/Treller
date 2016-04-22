using System.Collections.Generic;
using System.Reflection;

namespace SKBKontur.Infrastructure.Common
{
    public interface IAssemblyService
    {
        IEnumerable<Assembly> GetLoadedAssemblies();
    }
}