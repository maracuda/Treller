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
        private readonly Type[] defaultTasksListBlocks = { typeof(BoardsBlock), typeof(CardListBlock) };

        public TasksListController(IBlocksBuilder blocksBuilder)
        {
            this.blocksBuilder = blocksBuilder;
        }

        public async Task<ActionResult> Index()
        {
            BaseCardListBlock[] bodyBlocks;
            BaseCardListBlock[] headerBlocks;

            var cardListEnterModel = new CardListEnterModel { BoardIds = new string[0] };
            (await blocksBuilder.BuildBlocks(ContextKeys.TasksKey, defaultTasksListBlocks, cardListEnterModel))
                .Cast<BaseCardListBlock>()
                .Split(block => block is CardListBlock, out bodyBlocks, out headerBlocks);
            
            return View("TasksList", new TaskListViewModel
            {
                HeaderBlocks = headerBlocks,
                TaskList = bodyBlocks.FirstOrDefault()
            });
        }

        [HttpGet]
        public async Task<ActionResult> TaskList()
        {
            BaseCardListBlock[] bodyBlocks;
            BaseCardListBlock[] headerBlocks;

            var cardListEnterModel = new CardListEnterModel { BoardIds = new string[0] };
            (await blocksBuilder.BuildBlocks(ContextKeys.TasksKey, defaultTasksListBlocks, cardListEnterModel))
                .Cast<BaseCardListBlock>()
                .Split(block => block is CardListBlock, out bodyBlocks, out headerBlocks);

            return Json(bodyBlocks.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }
    }
}