using System;
using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Repository;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class RepositoryController : Controller
    {
        private readonly IOldBranchesModelBuilder oldBranchesModelBuilder;

        public RepositoryController(IOldBranchesModelBuilder oldBranchesModelBuilder)
        {
            this.oldBranchesModelBuilder = oldBranchesModelBuilder;
        }

        public ActionResult Index()
        {
            var oldBranchesModel = new OldBranchesModel
            {
                TotalNumber = 100,
            };//oldBranchesModelBuilder.Build(TimeSpan.FromDays(30), TimeSpan.FromDays(60));
            return View("Index", oldBranchesModel);
        }
    }
}