using System;
using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface IChecklistParrotsBuilder
    {
        ParrotsInfoViewModel Build(IEnumerable<CardChecklist> checklists, int daysCount, DateTime? beginDate, DateTime? endDate);
    }
}