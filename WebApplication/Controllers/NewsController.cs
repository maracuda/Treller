using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class NewsController : ExceptionHandledController
    {
        private readonly IBillingTimes billingTimes;

        public NewsController(
            IBillingTimes billingTimes,
            IErrorService errorService) : base(errorService)
        {
            this.billingTimes = billingTimes;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var news = new NewsViewModel
            {
                TaskNews = billingTimes.SelectAll()
            };
            return View("Index", news);
        }

        public ActionResult Deliver(string taskId, NewDeliveryChannelType deliveryChannel)
        {
            billingTimes.Publish(taskId, deliveryChannel);
            return RedirectToAction("Index");
        }

        public ActionResult TryToRequestNew(string aboutCardId)
        {
            billingTimes.TryToRequestNew(aboutCardId);
            return RedirectToAction("Index");
        }
    }

    public class NewsViewModel
    {
        public TaskNewModel[] TaskNews { get; set; }
    }
}