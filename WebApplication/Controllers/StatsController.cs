using System.Web.Mvc;
using Logger;
using ProcessStats.Dev;

namespace WebApplication.Controllers
{
    public class StatsController : ExceptionHandledController
    {
        private readonly ICardStatsBuilder cardStatsBuilder;

        public StatsController(
            ICardStatsBuilder cardStatsBuilder,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.cardStatsBuilder = cardStatsBuilder;
        }

        [HttpGet]
        public ActionResult Index(string taskId)
        {
            var statsModel = cardStatsBuilder.Build(taskId);
            return View("Index", statsModel);
        }
    }
}