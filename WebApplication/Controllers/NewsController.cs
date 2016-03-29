using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;

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
            var news = newsService.GetNews();
            return View("Index", news);
        }

        [HttpGet]
        public ActionResult SendTechnicalNews()
        {
            newsService.SendTechnicalNews();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SendNews()
        {
            newsService.SendNews();

            return RedirectToAction("Index");
        }

        public ActionResult ActualizeNews()
        {
            newsService.Refresh();

            return RedirectToAction("Index");
        }

        public ActionResult RestoreCardForNews(string cardId)
        {
            newsService.RestoreCard(cardId);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteCardFromNews(string cardId)
        {
            newsService.DeleteCard(cardId);

            return RedirectToAction("Index");
        }

        public ActionResult UpdateTechnicalEmail(string technicalEmail, string releaseEmail)
        {
            newsService.UpdateEmail(technicalEmail, releaseEmail);

            return RedirectToAction("Index");
        }

        public ActionResult ResetBattleEmails()
        {
            newsService.UpdateEmailToBattleValues();

            return RedirectToAction("Index");
        }
    }
}