using System;
using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Services.News;
using SKBKontur.Treller.WebApplication.Services.TaskCacher;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService newsService;
        private readonly IOperationalService operationalService;

        public NewsController(INewsService newsService, IOperationalService operationalService)
        {
            this.newsService = newsService;
            this.operationalService = operationalService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var news = newsService.GetAllNews();
            return View("Index", news);
        }

        [HttpGet]
        public ActionResult SendNews(Guid id)
        {
            newsService.SendNews(id);

            return RedirectToAction("Index");
        }

        public ActionResult ActualizeNews()
        {
            operationalService.Actualize();
            newsService.TryRefresh(DateTime.Today);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteNews(Guid id)
        {
            newsService.DeleteNews(id);

            return RedirectToAction("Index");
        }
    }
}