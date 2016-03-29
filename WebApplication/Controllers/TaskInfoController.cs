using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.BlocksMapping.Blocks;
using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Blocks;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Blocks;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Blocks.Parts;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class TaskInfoController : Controller
    {
        private readonly IBlocksBuilder blocksBuilder;

        private readonly Type[] defaultTaskInfoBlocks =
        {
            typeof (CardNameBlock),
            typeof (CardAvatarBlock), 
            typeof (CardLabelsBlock), 
            typeof (CardStateBlock),
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