using System.Collections.Generic;
using SKBKontur.Billy.Core.BlocksMapping.Abstrations;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization
{
    public class TaskDetalizationKeyInitialNamer : IContextKeyInitialParameterNamer
    {
        public IEnumerable<string> GetParamterNames()
        {
            return new[] { "cardId" };
        }

        public string ContextKey { get { return ContextKeys.TaskDetalizationKey; } }
    }
}