using System;
using System.Web.Mvc;
using Logger;
using RepositoryHooks.BranchNotification;

namespace WebApplication.Controllers
{
    public class RepositoryController : ExceptionHandledController
    {
        private readonly IOldBranchesModelBuilder oldBranchesModelBuilder;
        private readonly IBranchNotificator branchNotificator;

        public RepositoryController(
            IOldBranchesModelBuilder oldBranchesModelBuilder,
            IBranchNotificator branchNotificator,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.oldBranchesModelBuilder = oldBranchesModelBuilder;
            this.branchNotificator = branchNotificator;
        }

        public ActionResult Index()
        {
            var oldBranchesModel = oldBranchesModelBuilder.Build(TimeSpan.FromDays(15));
            return View("Index", oldBranchesModel);
        }

        public ActionResult NotifyCommitersAboutOldBranches()
        {
            branchNotificator.NotifyCommitersAboutIdlingBranches(TimeSpan.FromDays(15));
            return RedirectToAction("Index");
        }

        public ActionResult NotifyCommitersAboutMergedBranches()
        {
            branchNotificator.NotifyCommitersAboutMergedBranches(TimeSpan.FromDays(15));
            return RedirectToAction("Index");
        }
    }
}