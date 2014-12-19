using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.Billy.Core.BlocksMapping.Blocks;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class TasksController : Controller
    {
        private readonly IBlocksBuilder blocksBuilder;
        private readonly IAssemblyService assemblyService;

        public TasksController(IBlocksBuilder blocksBuilder, IAssemblyService assemblyService)
        {
            this.blocksBuilder = blocksBuilder;
            this.assemblyService = assemblyService;
        }

        public async Task<ActionResult> Index()
        {
            var blockTypes = new[] { typeof(BoardsBlock), typeof(CardOverallBlock), typeof(CardListBlock) };
            var blocks = (await blocksBuilder.BuildBlocks(ContextKeys.TasksKey, blockTypes, new CardListEnterModel { BoardIds = new string[0] })).Cast<BaseCardListBlock>().ToArray();

            return View("BaseCardListBlocks", blocks);
        }

        public async Task<ActionResult> GetDetalization(string cardId)
        {
            var blocks = assemblyService.GetAllDerivedTypes(typeof(BaseTaskDetalizationBlock)).Where(x => x != typeof(BasePartTaskDetalizationBlock)).ToArray();
            var result = (await blocksBuilder.BuildBlocks(ContextKeys.TaskDetalizationKey, blocks, cardId)).Cast<BaseTaskDetalizationBlock>().ToArray();
            var partBlocks = result.Where(x => x.GetType().IsSubclassOf(typeof (BasePartTaskDetalizationBlock))).ToArray();
            var commonBlocks = result.Except(partBlocks).ToArray();

            return View("TaskDetalization", new TaskDetalizationViewModel
                                                {
                                                    CardId = cardId,
                                                    CommonBlocks = commonBlocks,
                                                    PartBlocks = partBlocks.Cast<BasePartTaskDetalizationBlock>().ToArray()
                                                });
        }
    }
}