using System;
using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters
{
    public interface ITaskNewConverter
    {
        List<TaskNew> Convert(BoardList boardList);
        List<TaskNew> Convert(string cardId, string cardName, string cardDesc, DateTime? cardDueDate);
    }
}