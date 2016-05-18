using System;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Index()
        {
            var oldBranchesModel = await oldBranchesModelBuilder.BuildAsync(TimeSpan.FromDays(30), TimeSpan.FromDays(60)).ConfigureAwait(false);
            return View("Index", oldBranchesModel);
        }
    }
}