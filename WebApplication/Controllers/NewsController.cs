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

        [HttpGet]
        public ActionResult GoToDetails(string taskId)
        {
            var taskNewModel = billingTimes.Read(taskId);
            return View("New", taskNewModel);
        }

        public ActionResult Publish(string taskId, PublishStrategy publishStrategy)
        {
            billingTimes.Publish(taskId, publishStrategy);
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