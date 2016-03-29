using System.Collections.Generic;
using System.Reflection;
using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.Infrastructure.Common;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions
{
    public class DomainService : IDomainService
    {
        private readonly IAssemblyService assemblyService;

        public DomainService(IAssemblyService assemblyService)
        {
            this.assemblyService = assemblyService;
        }

        public IEnumerable<Assembly> GetSystemAssemblies()
        {
            return assemblyService.GetLoadedAssemblies();
        }
    }
}