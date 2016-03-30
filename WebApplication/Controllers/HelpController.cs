using System.Web.Mvc;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Help;
using SKBKontur.Treller.WebApplication.Implementation.Services.Notifications;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class HelpController : Controller
    {
        private readonly IBugTrackerClient bugTrackerClient;
        private readonly INotificationService notificationService;

        public HelpController(IBugTrackerClient bugTrackerClient, INotificationService notificationService)
        {
            this.bugTrackerClient = bugTrackerClient;
            this.notificationService = notificationService;
        }

        public ActionResult Index()
        {
            var notificationRecipient = notificationService.GetNotificationRecipient();

            return View("Index", new HelpViewModel
            {
                NotificationRecipientEmail = notificationRecipient
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
            notificationService.ChangeNotificationRecipient(email);

            return RedirectToAction("Index");
        }

        public ActionResult SendNotification(string text)
        {
            notificationService.SendErrorReport(text, null);

            return RedirectToAction("Index");
        }
    }
}