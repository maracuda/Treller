using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Search;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class NewsController : ExceptionHandledController
    {
        private readonly INewsFeed newsFeed;
        private readonly IPublisher publisher;

        public NewsController(
            INewsFeed newsFeed,
            IPublisher publisher,
            IErrorService errorService) : base(errorService)
        {
            this.newsFeed = newsFeed;
            this.publisher = publisher;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var news = new NewsViewModel
            {
                TaskNews = newsFeed.SelectAll()
            };
            return View("Index", news);
        }

        public ActionResult Deliver(string taskId)
        {
            publisher.Publish(taskId);
            return RedirectToAction("Index");
        }
    }

    public class NewsViewModel
    {
        public TaskNewModel[] TaskNews { get; set; }
    }
}