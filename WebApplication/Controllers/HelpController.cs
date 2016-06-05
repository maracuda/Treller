using System.Web.Mvc;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Help;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class HelpController : ExceptionHandledController
    {
        private readonly IBugTrackerClient bugTrackerClient;

        public HelpController(
            IBugTrackerClient bugTrackerClient,
            IErrorService errorService) : base(errorService)
        {
            this.bugTrackerClient = bugTrackerClient;
        }

        public ActionResult Index()
        {
            return View("Index", new HelpViewModel
            {
                NotificationRecipientEmail = errorService.ErrorRecipientEmail
            });
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

        public ActionResult UpdateNotificationEmail(string email)
        {
            errorService.ChangeErrorRecipientEmail(email);
            return RedirectToAction("Index");
        }

        public ActionResult SendNotification(string text)
        {
            errorService.SendError(text, null);
            return RedirectToAction("Index");
        }
    }
}