using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SKBKontur.Billy.Core.BlocksMapping.Abstrations;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks;

namespace SKBKontur.Treller.WebApplication.Abstractions
{
    public class DomainService : IDomainService
    {
        private readonly IAssemblyService assemblyService;
        private Type[] detalizationBlocks;

        public DomainService(IAssemblyService assemblyService)
        {
            this.assemblyService = assemblyService;
        }

        public IEnumerable<Assembly> GetSystemAssemblies()
        {
            return assemblyService.GetLoadedAssemblies();
        }

        public Type[] GetAllCardDetalizationBlockTypes()
        {
            detalizationBlocks = detalizationBlocks 
                                  ?? assemblyService.GetLoadedAssemblies()
                                                    .SelectMany(a => a.GetTypes().Where(x => x.IsSubclassOf(typeof (BaseTaskDetalizationBlock))))
                                                    .Distinct()
                                                    .ToArray();
            return detalizationBlocks;
        }
    }
}