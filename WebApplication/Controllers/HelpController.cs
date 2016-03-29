using System.Web.Mvc;
using SKBKontur.TaskManagerClient;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class HelpController : Controller
    {
        private readonly IBugTrackerClient bugTrackerClient;

        public HelpController(IBugTrackerClient bugTrackerClient)
        {
            this.bugTrackerClient = bugTrackerClient;
        }

        public ActionResult Index()
        {
            return View("Index");
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
    }
}