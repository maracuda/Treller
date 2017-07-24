using System.Web.Mvc;
using Logger;
using WebApplication.Implementation.Services.News;
using WebApplication.Implementation.Services.News.NewsFeed;

namespace WebApplication.Controllers
{
    public class NewsController : ExceptionHandledController
    {
        private readonly IBillingTimes billingTimes;

        public NewsController(
            IBillingTimes billingTimes,
            ILoggerFactory loggerFactory) : base(loggerFactory)
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