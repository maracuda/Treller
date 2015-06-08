using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SKBKontur.BlocksMapping.Blocks;
using SKBKontur.Treller.WebApplication.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool.Models;
using SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool.ViewModels;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class PeopleLoadPoolController : Controller
    {
        private readonly IBlocksBuilder blocksBuilder;
        private readonly Type[] defaultBlocks = { typeof(PeopleLoadPoolListBlock) };

        public PeopleLoadPoolController(IBlocksBuilder blocksBuilder)
        {
            this.blocksBuilder = blocksBuilder;
        }

        public async Task<ActionResult> Index()
        {
            var poolEnterModel = new PeopleLoadPoolEnterModel { BoardIds = new string[0] };
            var blocks = (await blocksBuilder.BuildBlocks(ContextKeys.PeopleLoadPoolKey, defaultBlocks, poolEnterModel)).Cast<BasePeopleLoadPoolBlock>().ToArray();
            var peopleLoadPoolViewModel = new PeopleLoadPoolViewModel { Peoples = ((PeopleLoadPoolListBlock)blocks.First(x => x is PeopleLoadPoolListBlock)).PeopleList };

            if (Request.IsAjaxRequest())
                return Json(peopleLoadPoolViewModel, JsonRequestBehavior.AllowGet);

            return View("PeopleLoadPool", peopleLoadPoolViewModel);
        }
    }
}