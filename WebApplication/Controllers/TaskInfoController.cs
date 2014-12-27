using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.Billy.Core.BlocksMapping.Blocks;
using SKBKontur.Treller.WebApplication.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class TaskInfoController : Controller
    {
        private readonly IBlocksBuilder blocksBuilder;

        private readonly Type[] defaultTaskInfoBlocks =
        {
            typeof (CardNameBlock), 
            typeof (CardAvatarBlock), 
            typeof (CardDescriptionBlock), 
            typeof (CardBranchBlock), 
            
            typeof (CardLabelsBlock), 
            typeof (CardStateBlock), 
            typeof (CardWorkBlock), 
            typeof (CardDetalizationPartsBlock)
        };

        public TaskInfoController(IBlocksBuilder blocksBuilder)
        {
            this.blocksBuilder = blocksBuilder;
        }

        public async Task<ActionResult> TaskInfo(string cardId)
        {
            var result = (await blocksBuilder.BuildBlocks(ContextKeys.TaskDetalizationKey, defaultTaskInfoBlocks, cardId)).Cast<BaseTaskDetalizationBlock>().ToArray();
            var detalizationBlock = (CardDetalizationPartsBlock)result.First(x => x is CardDetalizationPartsBlock);
            var commonBlocks = result.Where(x => x != detalizationBlock).ToArray();

            return PartialView("TaskInfo", new TaskDetalizationViewModel
            {
                CardId = cardId,
                CommonBlocks = commonBlocks,
                Detalization = detalizationBlock
            });
        }
    }
}