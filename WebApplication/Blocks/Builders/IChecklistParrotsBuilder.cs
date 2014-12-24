using System;
using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Cards;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface IChecklistParrotsBuilder
    {
        ParrotsInfo Build(IEnumerable<CardChecklist> checklists, int daysCount);
    }
}