using System.Collections.Generic;
using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskDetalization
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