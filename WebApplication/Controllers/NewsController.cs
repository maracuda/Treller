using System;
using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Services.News;
using SKBKontur.Treller.WebApplication.Services.TaskCacher;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService newsService;
        private readonly ITaskCacher taskCacher;

        public NewsController(INewsService newsService, ITaskCacher taskCacher)
        {
            this.newsService = newsService;
            this.taskCacher = taskCacher;
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
            newsService.TryRefresh(DateTime.Now.Date);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteNews(Guid id)
        {
            newsService.DeleteNews(id);

            return RedirectToAction("Index");
        }
    }
}