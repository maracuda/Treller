using System;
using SKBKontur.Billy.Core.BlocksMapping.Abstrations;
using SKBKontur.Infrastructure.ContainerConfiguration;

namespace SKBKontur.Treller.WebApplication.Services.Abstractions
{
    public class IocContainer : IIocContainer
    {
        private readonly IContainer container;

        public IocContainer(IContainer container)
        {
            this.container = container;
        }

        public object Get(Type declaringType)
        {
            return container.Get(declaringType);
        }
    }
}