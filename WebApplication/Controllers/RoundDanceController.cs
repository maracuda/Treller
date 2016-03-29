using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Services.RoundDance;

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
            var viewModel = roundDanceViewModelBuilder.BuildWithLinks();
            return View("Index", viewModel);
        }

        [HttpGet]
        public ActionResult Duty()
        {
            var viewModel = roundDanceViewModelBuilder.Build();
            return View("Duty", viewModel);
        }
    }
}