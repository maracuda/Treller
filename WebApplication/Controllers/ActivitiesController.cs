using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Fan.Activities;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly IDepartureEventStorage departureEventStorage;

        public ActivitiesController(IDepartureEventStorage departureEventStorage)
        {
            this.departureEventStorage = departureEventStorage;
        }

        public ActionResult Index()
        {
            var nextEvent = departureEventStorage.GetNextEvent();

            return View("Index", nextEvent);
        }
    }
}