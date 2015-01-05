using System.Collections.Generic;
using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList
{
    public class TasksListInitialNamer : IContextKeyInitialParameterNamer
    {
        public IEnumerable<string> GetParamterNames()
        {
            return new[] { typeof(CardListEnterModel).FullName };
        }

        public string ContextKey { get { return ContextKeys.TasksKey; } }
    }
}