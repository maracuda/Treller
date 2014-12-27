using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface IChecklistParrotsBuilder
    {
        ParrotsInfo Build(IEnumerable<CardChecklist> checklists, int daysCount);
    }
}