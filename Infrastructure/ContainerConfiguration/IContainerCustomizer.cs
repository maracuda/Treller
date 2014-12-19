namespace SKBKontur.Infrastructure.ContainerConfiguration
{
    public interface IContainerCustomizer
    {
        void Customize(IContainer container);
    }
}