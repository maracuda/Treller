using System;
using System.Globalization;
using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.RoundDance;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class RoundDanceController : Controller
    {
        private readonly IRoundDanceViewModelBuilder roundDanceViewModelBuilder;
        private readonly IRoundDancePeopleStorage roundDancePeopleStorage;

        public RoundDanceController(IRoundDanceViewModelBuilder roundDanceViewModelBuilder, IRoundDancePeopleStorage roundDancePeopleStorage)
        {
            this.roundDanceViewModelBuilder = roundDanceViewModelBuilder;
            this.roundDancePeopleStorage = roundDancePeopleStorage;
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

        [HttpPost]
        public ActionResult AddRoundDance(string name, string direction, string beginDate, string pairName, string email)
        {
            var beginDateResult = ParseToDateTime(beginDate);
            if (beginDateResult.HasValue)
            {
                roundDancePeopleStorage.AddNew(name, direction, beginDateResult.Value, email, pairName);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromNextRoundDance(string name, string direction, string beginDate)
        {
            var beginDateResult = ParseToDateTime(beginDate);
            if (beginDateResult.HasValue)
            {
                roundDancePeopleStorage.Delete(name, direction, beginDateResult.Value);
            }

            return RedirectToAction("Index");
        }

        private DateTime? ParseToDateTime(string date)
        {
            DateTime result;
            if (DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            return null;
        }
    }
}