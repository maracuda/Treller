using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Sender;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class NewsController : ExceptionHandledController
    {
        private readonly INewsModelBuilder newsModelBuilder;
        private readonly IMagazine magazine;

        public NewsController(
            INewsModelBuilder newsModelBuilder,
            IMagazine magazine,
            IErrorService errorService) : base(errorService)
        {
            this.newsModelBuilder = newsModelBuilder;
            this.magazine = magazine;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var news = newsModelBuilder.BuildViewModel();
            return View("Index", news);
        }

        public ActionResult Deliver(string taskId)
        {
            magazine.Publish(taskId);
            return RedirectToAction("Index");
        }
    }
}