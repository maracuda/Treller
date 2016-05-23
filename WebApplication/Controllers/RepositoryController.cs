using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Repository;
using SKBKontur.Treller.WebApplication.Implementation.Services.Notifications;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class RepositoryController : Controller
    {
        private readonly IOldBranchesModelBuilder oldBranchesModelBuilder;
        private readonly INotificationService notificationService;
        private readonly INotificationBuilder notificationBuilder;

        public RepositoryController(
            IOldBranchesModelBuilder oldBranchesModelBuilder,
            INotificationService notificationService,
            INotificationBuilder notificationBuilder)
        {
            this.oldBranchesModelBuilder = oldBranchesModelBuilder;
            this.notificationService = notificationService;
            this.notificationBuilder = notificationBuilder;
        }

        public ActionResult Index()
        {
            var oldBranchesModel = oldBranchesModelBuilder.Build(TimeSpan.FromDays(15));
            return View("Index", oldBranchesModel);
        }

        public ActionResult NotifyCommitersAboutOldBranches()
        {
            var commiterIndex = new Dictionary<string, List<string>>();
            var oldBranchesModel = oldBranchesModelBuilder.Build(TimeSpan.FromDays(15));
            foreach (var veryOldBranch in oldBranchesModel.OldBracnhes)
            {
                if (!commiterIndex.ContainsKey(veryOldBranch.Commit.Committer_email))
                {
                    commiterIndex.Add(veryOldBranch.Commit.Committer_email, new List<string>());
                }
                commiterIndex[veryOldBranch.Commit.Committer_email].Add(veryOldBranch.Name);
            }

            foreach (var emailToBranchesPair in commiterIndex)
            {
                var notification = notificationBuilder.BuildForOldBranchNotification(emailToBranchesPair.Key, emailToBranchesPair.Value);
                notificationService.Send(notification);
            }

            return RedirectToAction("Index");
        }
    }
}