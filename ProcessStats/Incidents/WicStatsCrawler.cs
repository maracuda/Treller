using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace ProcessStats.Incidents
{
    public partial class WicStatsCrawler : IIncidentsStatsCrawler
    {
        private const string incomingIncidentsUrl = "http://wicstat/ReportServer/Pages/ReportViewer.aspx?%2FWic2Reports%2F%D0%AD%D0%BA%D1%81%D0%BF%D0%B5%D1%80%D1%82%D0%BD%D1%8B%D0%B9%20%D0%BE%D1%82%D0%B4%D0%B5%D0%BB%2F%D0%93%D1%80%D1%83%D0%BF%D0%BF%D0%BE%D0%B2%D1%8B%D0%B5%20%D1%82%D0%B5%D0%B3%D0%B8%20%D0%AD%D0%9E%20%D0%BF%D0%BE%20%D0%B4%D0%B0%D1%82%D0%B5%20%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4%D0%B0&rc:showbackbutton=true";
        private const string fixedIncidentsUrl = "http://wicstat/ReportServer/Pages/ReportViewer.aspx?%2FWic2Reports%2F%D0%AD%D0%BA%D1%81%D0%BF%D0%B5%D1%80%D1%82%D0%BD%D1%8B%D0%B9%20%D0%BE%D1%82%D0%B4%D0%B5%D0%BB%2F%D0%AD%D0%BA%D1%81%D0%BF%D0%B5%D1%80%D1%82%D0%BD%D1%8B%D0%B9%20%D0%BE%D1%82%D0%B4%D0%B5%D0%BB.%20%D0%93%D1%80%D1%83%D0%BF%D0%BF%D0%BE%D0%B2%D1%8B%D0%B5%20%D1%82%D0%B5%D0%B3%D0%B8&rc:showbackbutton=true";

        public IncidentsStats Collect(DateTime date)
        {
            var driver = new ChromeDriver();
            var incomingCount = CollectCounter(driver, incomingIncidentsUrl, date);
            var fixedCount = CollectCounter(driver, fixedIncidentsUrl, date);
            driver.Quit();

            return new IncidentsStats
            {
                IncomingCount = incomingCount,
                FixedCount = fixedCount,
                Date = date.AddDays(-1)
            };
        }

        private static int CollectCounter(RemoteWebDriver driver, string url, DateTime date)
        {
            for (var i = 0; i < 3; i++)
            {
                try
                {
                    return WicReportPage.Create(driver, url)
                                        .InputBeginDate(date)
                                        .InputEndDate(date)
                                        .ClickSearchButton()
                                        .GetTotalCount();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            throw new Exception("Fail to collect counter.");
        }
    }
}