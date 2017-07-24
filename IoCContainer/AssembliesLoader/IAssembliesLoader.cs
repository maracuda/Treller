using System.Collections.Generic;
using System.Reflection;

namespace IoCContainer.AssembliesLoader
{
    public interface IAssembliesLoader
    {
        IEnumerable<Assembly> LoadAssemblies();
    }
}