using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.BlocksMapping.Blocks;
using SKBKontur.Treller.WebApplication.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using System.Linq;
using SKBKontur.Treller.WebApplication.Extensions;
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

        public async Task<ActionResult> Index()
        {
            var taskListViewModel = await CreateTaskListViewModel();

            if (Request.IsAjaxRequest())
                return Json(taskListViewModel, JsonRequestBehavior.AllowGet);

            return View("TasksList", taskListViewModel);
        }

        private async Task<TaskListViewModel> CreateTaskListViewModel()
        {
            var cardListEnterModel = new CardListEnterModel {BoardIds = new string[0]};
            var blocks = (await blocksBuilder.BuildBlocks(ContextKeys.TasksKey, defaultTasksListBlocks, cardListEnterModel))
                .Cast<BaseCardListBlock>()
                .ToArray();

            return new TaskListViewModel
            {
                BoardsBlock = blocks.FirstOrDefault(x => x is BoardsBlock),
                TaskList = blocks.FirstOrDefault(x => x is CardListBlock)
            };
        }
    }
}