using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public abstract class ExceptionHandledController : Controller
    {
        protected readonly IErrorService errorService;

        protected ExceptionHandledController(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            errorService.SendError("Controller error", filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}