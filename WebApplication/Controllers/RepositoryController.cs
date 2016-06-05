using System;
using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Repository;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class RepositoryController : ExceptionHandledController
    {
        private readonly IOldBranchesModelBuilder oldBranchesModelBuilder;
        private readonly IRepositoryNotificator repositoryNotificator;

        public RepositoryController(
            IOldBranchesModelBuilder oldBranchesModelBuilder,
            IRepositoryNotificator repositoryNotificator,
            IErrorService errorService) : base(errorService)
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