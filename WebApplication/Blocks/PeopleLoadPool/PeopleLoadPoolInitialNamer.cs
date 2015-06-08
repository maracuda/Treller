using System.Collections.Generic;
using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool.Models;
using SKBKontur.BlocksMapping.BlockExtenssions;

namespace SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool
{
    public class PeopleLoadPoolInitialNamer : IContextKeyInitialParameterNamer
    {
        public IEnumerable<string> GetParamterNames()
        {
            return new[] { typeof(PeopleLoadPoolEnterModel).ToBlockParameterKey() };
        }

        public string ContextKey { get { return ContextKeys.PeopleLoadPoolKey; } }
    }
}