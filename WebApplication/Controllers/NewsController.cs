using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Services;
using SKBKontur.Treller.WebApplication.Services.News;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var news = newsService.GetAllNews();
            return View("Index", news);
        }
    }
}