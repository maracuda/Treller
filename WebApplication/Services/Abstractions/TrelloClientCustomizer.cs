using SKBKontur.Infrastructure.ContainerConfiguration;

namespace SKBKontur.Treller.WebApplication.Services.Abstractions
{
    public class TrelloClientCustomizer : IContainerCustomizer
    {
        public void Customize(IContainer container)
        {
            container.RegisterType<TrelloClient.TrelloClient>();
        }
    }
}