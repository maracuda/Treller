using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface IBugsBuilder
    {
        BugsInfoViewModel Build(IEnumerable<CardChecklist> checklists);
    }
}