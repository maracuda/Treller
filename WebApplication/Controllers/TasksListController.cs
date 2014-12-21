using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.Billy.Core.BlocksMapping.Blocks;
using SKBKontur.Treller.WebApplication.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class TasksListController : Controller
    {
        private readonly IBlocksBuilder blocksBuilder;
        private readonly Type[] defaultTasksListBlocks = { typeof(BoardsBlock), typeof(CardOverallBlock), typeof(CardListBlock) };

        public TasksListController(IBlocksBuilder blocksBuilder)
        {
            this.blocksBuilder = blocksBuilder;
        }

        public async Task<ActionResult> Index()
        {
            var cardListEnterModel = new CardListEnterModel { BoardIds = new string[0] };
            var blocks = (await blocksBuilder.BuildBlocks(ContextKeys.TasksKey, defaultTasksListBlocks, cardListEnterModel)).Cast<BaseCardListBlock>().ToArray();

            return View("TasksList", blocks);
        }
    }
}