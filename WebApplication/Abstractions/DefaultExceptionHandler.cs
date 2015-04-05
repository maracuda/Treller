using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.BlocksMapping.Mappings;

namespace SKBKontur.Treller.WebApplication.Abstractions
{
    public class DefaultExceptionHandler : IBlocksExceptionHandler
    {
        public string[] GetContextKeys()
        {
            return null;
        }

        public void HandleSubBuildException(string contextKey, BlockSubBuildException exception)
        {
        }
    }
}