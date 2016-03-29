using System;
using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager
{
    public interface IChecklistParrotsBuilder
    {
        ParrotsInfoViewModel Build(IEnumerable<CardChecklist> checklists, int daysCount, DateTime? beginDate, DateTime? endDate);
    }
}