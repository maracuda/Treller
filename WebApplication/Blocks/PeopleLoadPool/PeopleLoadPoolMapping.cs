using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.BlocksMapping.Mappings;
using SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool
{
    public class PeopleLoadPoolMapping : IContextBlocksMapping
    {
        public IBlockMapper[] SelectAll()
        {
            return new IBlockMapper[]
                       {
                           BlockMapper.Declare<PeopleLoadPoolListBlock, PeopleSimpleViewModel[]>(x => x.PeopleList)
                       };
        }

        public string GetContextKey()
        {
            return ContextKeys.PeopleLoadPoolKey;
        }
    }
}