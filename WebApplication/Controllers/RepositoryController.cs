using System;
using System.Web.Mvc;
using SKBKontur.Treller.Logger;
using SKBKontur.Treller.WebApplication.Implementation.Repository;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class RepositoryController : ExceptionHandledController
    {
        private readonly IOldBranchesModelBuilder oldBranchesModelBuilder;
        private readonly IRepositoryNotificator repositoryNotificator;

        public RepositoryController(
            IOldBranchesModelBuilder oldBranchesModelBuilder,
            IRepositoryNotificator repositoryNotificator,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.oldBranchesModelBuilder = oldBranchesModelBuilder;
            this.repositoryNotificator = repositoryNotificator;
        }

        public ActionResult Index()
        {
            var oldBranchesModel = oldBranchesModelBuilder.Build(TimeSpan.FromDays(15));
            return View("Index", oldBranchesModel);
        }

        public ActionResult NotifyCommitersAboutOldBranches()
        {
            repositoryNotificator.NotifyCommitersAboutIdlingBranches(TimeSpan.FromDays(15));
            return RedirectToAction("Index");
        }

        public ActionResult NotifyCommitersAboutMergedBranches()
        {
            repositoryNotificator.NotifyCommitersAboutMergedBranches(TimeSpan.FromDays(15));
            return RedirectToAction("Index");
        }
    }
}