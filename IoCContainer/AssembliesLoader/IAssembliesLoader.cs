using System.Collections.Generic;
using System.Reflection;

namespace SKBKontur.Treller.IoCContainer.AssembliesLoader
{
    public interface IAssembliesLoader
    {
        IEnumerable<Assembly> LoadAssemblies();
    }
}