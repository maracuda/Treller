using System.Collections.Generic;
using System.Reflection;
using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.IoCContainer.AssembliesLoader;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions
{
    public class DomainService : IDomainService
    {
        private readonly IAssembliesLoader assembliesLoader;

        public DomainService(IAssembliesLoader assembliesLoader)
        {
            this.assembliesLoader = assembliesLoader;
        }

        public IEnumerable<Assembly> GetSystemAssemblies()
        {
            return assembliesLoader.LoadAssemblies();
        }
    }
}