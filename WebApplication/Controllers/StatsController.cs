using System.Web.Mvc;
using SkbKontur.Treller.ProcessStats.Dev;
using SKBKontur.Treller.Logger;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class StatsController : ExceptionHandledController
    {
        private readonly IKanbanStats kanbanStats;

        public StatsController(
            IKanbanStats kanbanStats,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.kanbanStats = kanbanStats;
        }

        [HttpGet]
        public ActionResult Index(string taskId)
        {
            var statsModel = kanbanStats.Build(taskId);
            return View("Index", statsModel);
        }
    }
}