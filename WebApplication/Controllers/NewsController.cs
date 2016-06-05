using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class NewsController : ExceptionHandledController
    {
        private readonly INewsService newsService;
        private readonly INewsModelBuilder newsModelBuilder;
        private readonly INewsSettingsService newsSettingsService;

        public NewsController(
            INewsService newsService,
            INewsModelBuilder newsModelBuilder,
            INewsSettingsService newsSettingsService,
            IErrorService errorService) : base(errorService)
        {
            this.newsService = newsService;
            this.newsModelBuilder = newsModelBuilder;
            this.newsSettingsService = newsSettingsService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var news = newsModelBuilder.Build();
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

        public ActionResult UpdateSettings(string technicalEmail, string releaseEmail)
        {
            newsSettingsService.Update(technicalEmail, releaseEmail);
            return RedirectToAction("Index");
        }

        public ActionResult ResetSettings()
        {
            newsSettingsService.Reset();
            return RedirectToAction("Index");
        }
    }
}