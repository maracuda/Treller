using System.Collections.Generic;
using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Blocks;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList
{
    public class TasksListInitialNamer : IContextKeyInitialParameterNamer
    {
        public IEnumerable<string> GetParamterNames()
        {
            return new[] { typeof(CardListEnterModel).ToBlockParameterKey() };
        }

        public string ContextKey { get { return ContextKeys.TasksKey; } }
    }
}