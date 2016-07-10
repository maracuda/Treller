using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public interface ITaskNewConverter
    {
        List<TaskNew> Convert(string boardId, BoardListCardInfo cardInfo);
    }
}