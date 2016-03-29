using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.BlocksMapping.Blocks;
using SKBKontur.Treller.WebApplication.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using System.Linq;
using SKBKontur.Treller.WebApplication.Models.TaskList;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class TasksListController : Controller
    {
        private readonly IBlocksBuilder blocksBuilder;
        private readonly Type[] defaultTasksListBlocks = { typeof(BoardsBlock), typeof(CardListBlock), typeof(BugsBlock) };


        public TasksListController(IBlocksBuilder blocksBuilder)
        {
            this.blocksBuilder = blocksBuilder;
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
            var cardListEnterModel = new CardListEnterModel { BoardIds = new string[0], ShowMode = showMode };
            var blocks = (await blocksBuilder.BuildBlocks(ContextKeys.TasksKey, defaultTasksListBlocks, cardListEnterModel))
                .Cast<BaseCardListBlock>()
                .ToArray();

            return new TaskListViewModel
            {
                BoardsBlock = blocks.FirstOrDefault(x => x is BoardsBlock),
                BugsBlock = blocks.FirstOrDefault(x => x is BugsBlock),
                TaskList = blocks.FirstOrDefault(x => x is CardListBlock)
            };
        }
    }
}