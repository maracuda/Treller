using System.Web.Mvc;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.Logger;
using SKBKontur.Treller.WebApplication.Implementation.Help;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class HelpController : ExceptionHandledController
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IBugTrackerClient bugTrackerClient;

        public HelpController(
            ILoggerFactory loggerFactory,
            IBugTrackerClient bugTrackerClient) : base(loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.bugTrackerClient = bugTrackerClient;
        }

        public ActionResult Index()
        {
            return View("Index", new HelpViewModel());
        }

        public ActionResult DeleteAllCommentsFromConsistencyBattle()
        {
            var issueId = "BILLY-6280";
            var comments = bugTrackerClient.GetComments(issueId);
            var attechments = bugTrackerClient.GetAttachments(issueId);

            foreach (var comment in comments)
            {
                bugTrackerClient.DeleteComment(issueId, comment.Id, true);
            }

            foreach (var attachment in attechments)
            {
                bugTrackerClient.DeleteAttachment(issueId, attachment.Id);
            }

            return RedirectToAction("Index");
        }

        public ActionResult SendNotification(string text)
        {
            loggerFactory.Get<HelpController>().LogError(text);
            return RedirectToAction("Index");
        }
    }
}