using System.Web.Mvc;
using Logger;
using ProcessStats.Dev;

namespace WebApplication.Controllers
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