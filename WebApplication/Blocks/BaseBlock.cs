using SKBKontur.Billy.Core.BlocksMapping.Abstrations;

namespace SKBKontur.Treller.WebApplication.Blocks
{
    public abstract class BaseBlock : IBlock
    {
        public string InternalName { get { return GetType().FullName; } }
    }
}