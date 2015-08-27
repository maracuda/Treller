using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Controllers.RoundDance;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class RoundDanceController : Controller
    {
        private readonly IRoundDanceViewModelBuilder roundDanceViewModelBuilder;

        public RoundDanceController(IRoundDanceViewModelBuilder roundDanceViewModelBuilder)
        {
            this.roundDanceViewModelBuilder = roundDanceViewModelBuilder;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = roundDanceViewModelBuilder.Build();
            return View("Index", viewModel);
        }
    }
}