using System.Web.Mvc;
using Logger;

namespace WebApplication.Controllers
{
    public abstract class ExceptionHandledController : Controller
    {
        private readonly ILoggerFactory loggerFactory;

        protected ExceptionHandledController(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            loggerFactory.Get<ExceptionHandledController>().LogError("Controller error", filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}