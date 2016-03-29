using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Activities;
using SKBKontur.Treller.WebApplication.Implementation.Activities.BusinessObjects;

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