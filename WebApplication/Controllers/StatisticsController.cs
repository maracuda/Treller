using System;
using System.Globalization;
using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Statistics;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class StatisticsController : ExceptionHandledController
    {
        private readonly IStatisticsService statisticsService;

        public StatisticsController(
            IStatisticsService statisticsService,
            IErrorService errorService) : base(errorService)
        {
            this.statisticsService = statisticsService;
        }

        
        [HttpGet]
        public ActionResult Index()
        {
            var result = statisticsService.GetStatistics(new DateTime(2015, 09, 15), new DateTime(2105, 10, 31));
            return View("Index", result);
        }

        [HttpPost]
        public ActionResult Index(string calculateDate, string calculateEndDate)
        {
            var calcDate = ParseDate(calculateDate, new DateTime(2015, 09, 15));
            var calcEndDate = ParseDate(calculateEndDate, new DateTime(2015, 10, 31));

            var result = statisticsService.GetStatistics(calcDate, calcEndDate, true);
            return View("Index", result);
        }

        private static DateTime ParseDate(string dateFormat, DateTime defaultDate)
        {
            DateTime calcDate;
            if (!DateTime.TryParseExact(dateFormat, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out calcDate))
            {
                calcDate = defaultDate;
            }
            return calcDate;
        }
    }
}