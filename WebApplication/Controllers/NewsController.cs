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
        private readonly INewsSettingsService newsSettingsService;

        public NewsController(
            INewsModelBuilder newsModelBuilder,
            IMagazine magazine,
            INewsSettingsService newsSettingsService,
            IErrorService errorService) : base(errorService)
        {
            this.newsModelBuilder = newsModelBuilder;
            this.magazine = magazine;
            this.newsSettingsService = newsSettingsService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var news = newsModelBuilder.BuildViewModel();
            return View("Index", news);
        }

        [HttpGet]
        public ActionResult Deliver(string taskId)
        {
            magazine.Publish(taskId);
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