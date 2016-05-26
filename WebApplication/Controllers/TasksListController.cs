using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class TasksListController : Controller
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly Type[] defaultTasksListBlocks = { typeof(BoardsBlock), typeof(CardListBlock), typeof(BugsBlock) };


        public TasksListController(ITaskManagerClient taskManagerClient)
        {
            this.taskManagerClient = taskManagerClient;
        }

        public async Task<ActionResult> Index(ShowMode showMode = ShowMode.All)
        {
            var taskListViewModel = new TaskListViewModel
            {
                BoardsBlock = await BuildBoardsBlock()
            };

            if (Request.IsAjaxRequest())
                return Json(taskListViewModel, JsonRequestBehavior.AllowGet);

            return View("TasksList", taskListViewModel);
        }

        private async Task<BoardsBlock> BuildBoardsBlock()
        {
            var boardsTask = taskManagerClient.GetOpenBoardsAsync("konturbilling");

            return new BoardsBlock
            {
                Boards = await boardsTask.ConfigureAwait(false)
            };
        }
    }
}