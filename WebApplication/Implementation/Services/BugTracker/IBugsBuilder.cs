using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.BugTracker
{
    public interface IBugsBuilder
    {
        BugsInfoViewModel Build(IEnumerable<CardChecklist> checklists);
    }
}