using SKBKontur.BlocksMapping.Abstrations;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Blocks
{
    public abstract class BaseBlock : IBlock
    {
        public string InternalName { get { return GetType().Name; } }
    }
}