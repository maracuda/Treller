using System;
using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.Infrastructure.ContainerConfiguration;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions
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