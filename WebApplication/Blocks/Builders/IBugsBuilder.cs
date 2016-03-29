using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface IBugsBuilder
    {
        BugsInfoViewModel Build(IEnumerable<CardChecklist> checklists);
    }
}