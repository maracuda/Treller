using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class NewsController : ExceptionHandledController
    {
        private readonly INewsModelBuilder newsModelBuilder;
        private readonly IPublisher publisher;

        public NewsController(
            INewsModelBuilder newsModelBuilder,
            IPublisher publisher,
            IErrorService errorService) : base(errorService)
        {
            this.newsModelBuilder = newsModelBuilder;
            this.publisher = publisher;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var news = newsModelBuilder.BuildViewModel();
            return View("Index", news);
        }

        public ActionResult Deliver(string taskId)
        {
            publisher.Publish(taskId);
            return RedirectToAction("Index");
        }
    }
}