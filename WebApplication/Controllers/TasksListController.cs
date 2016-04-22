using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.BlocksMapping.Blocks;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Services.Repository;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class TasksListController : Controller
    {
        private readonly IRepoService repoService;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly Type[] defaultTasksListBlocks = { typeof(BoardsBlock), typeof(CardListBlock), typeof(BugsBlock) };


        public TasksListController(
            IRepoService repoService,
            ITaskManagerClient taskManagerClient)
        {
            this.repoService = repoService;
            this.taskManagerClient = taskManagerClient;
        }

        public async Task<ActionResult> Index(ShowMode showMode = ShowMode.All)
        {
            var taskListViewModel = await CreateTaskListViewModel(showMode);

            if (Request.IsAjaxRequest())
                return Json(taskListViewModel, JsonRequestBehavior.AllowGet);

            return View("TasksList", taskListViewModel);
        }

        private async Task<TaskListViewModel> CreateTaskListViewModel(ShowMode showMode)
        {
            /*var cardListEnterModel = new CardListEnterModel { BoardIds = new string[0], ShowMode = showMode };
            var blocks = (await blocksBuilder.BuildBlocks(ContextKeys.TasksKey, defaultTasksListBlocks, cardListEnterModel))
                .Cast<BaseCardListBlock>()
                .ToArray();*/

            return new TaskListViewModel
            {
                BoardsBlock = BuildBoardsBlock(),
                //BugsBlock = blocks.FirstOrDefault(x => x is BugsBlock),
                //TaskList = blocks.FirstOrDefault(x => x is CardListBlock)
            };
        }

        private BoardsBlock BuildBoardsBlock()
        {
            return new BoardsBlock
            {
                BranchesMergedToReleaseCandidate = repoService.SelectBranchesMergedToReleaseCandidate()
            };
        }
    }
}