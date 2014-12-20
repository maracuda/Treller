using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.Billy.Core.BlocksMapping.Blocks;
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

        public TasksController(IBlocksBuilder blocksBuilder)
        {
            this.blocksBuilder = blocksBuilder;
        }

        public async Task<ActionResult> Index()
        {
            var blockTypes = new[] { typeof(BoardsBlock), typeof(CardOverallBlock), typeof(CardListBlock) };
            var blocks = (await blocksBuilder.BuildBlocks(ContextKeys.TasksKey, blockTypes, new CardListEnterModel { BoardIds = new string[0] })).Cast<BaseCardListBlock>().ToArray();

            return View("BaseCardListBlocks", blocks);
        }

        public async Task<ActionResult> GetDetalization(string cardId)
        {
            var blocks = new[]
                            {
                                typeof (CardAvatarBlock), typeof (CardDescriptionBlock), typeof (CardBranchBlock), typeof (CardNameBlock), typeof (CardLabelsBlock), typeof (CardStateBlock), typeof (CardWorkBlock), typeof (CardDetalizationPartsBlock)
                            };
            var result = (await blocksBuilder.BuildBlocks(ContextKeys.TaskDetalizationKey, blocks, cardId)).Cast<BaseTaskDetalizationBlock>().ToArray();
            var detalizationBlock = (CardDetalizationPartsBlock)result.First(x => x is CardDetalizationPartsBlock);
            var commonBlocks = result.Where(x => x != detalizationBlock).ToArray();

            return View("TaskDetalization", new TaskDetalizationViewModel
                                                {
                                                    CardId = cardId,
                                                    CommonBlocks = commonBlocks,
                                                    Detalization = detalizationBlock
                                                });
        }
    }
}