using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;

namespace ProcessStats.Incidents
{
    public partial class WicStatsCrawler
    {
        public class WicReportPage
        {
            private readonly RemoteWebDriver driver;

            private WicReportPage(RemoteWebDriver driver)
            {
                this.driver = driver;
            }

            public static WicReportPage Create(RemoteWebDriver driver, string url)
            {
                driver.Navigate().GoToUrl(url);
                WaitForRefreshTable(driver);
                return new WicReportPage(driver);
            }

            public WicReportPage InputBeginDate(DateTime beginDate)
            {
                var start = "start";
                if (driver.FindElementsByCssSelector("[data-parametername='BeginDate']").Count > 0)
                {
                    start = "BeginDate";
                }
                var beginDateInput = driver.FindElementByCssSelector($"[data-parametername='{start}']")
                    .FindElement(By.Id("ReportViewerControl_ctl04_ctl03_txtValue"));
                beginDateInput.Clear();
                Thread.Sleep(1500);
                beginDateInput.SendKeys(beginDate.ToShortDateString() + Keys.Tab);
                WaitForRefreshTable(driver);
                return this;
            }

            public WicReportPage InputEndDate(DateTime endDate)
            {
                var end = "end";
                if (driver.FindElementsByCssSelector("[data-parametername='EndDate']").Count > 0)
                {
                    end = "EndDate";
                }
                var endDateInput = driver.FindElementByCssSelector($"[data-parametername='{end}']")
                    .FindElement(By.Id("ReportViewerControl_ctl04_ctl05_txtValue"));
                endDateInput.Clear();
                Thread.Sleep(1500);
                endDateInput.SendKeys(endDate.ToShortDateString() + Keys.Tab);
                WaitForRefreshTable(driver);
                return this;
            }

            public WicReportPage ClickSearchButton()
            {
                var searchButton = driver.FindElementByCssSelector("[id='ReportViewerControl_ctl04_ctl00']");
                searchButton.Click();
                WaitForRefreshTable(driver);
                return this;
            }

            public int GetTotalCount()
            {
                if (driver.FindElementById("ReportViewerControl_ctl05_ctl00_Last_ctl00_ctl00").Displayed)
                {
                    driver.FindElementById("ReportViewerControl_ctl05_ctl00_Last_ctl00_ctl00").Click();
                }
                WaitForRefreshTable(driver);
                return int.Parse(driver.FindElementByCssSelector("[id$='79iT0_aria']").Text);
            }

            private static void WaitForRefreshTable(IFindsById driver)
            {
                var element = driver.FindElementById("ReportViewerControl_AsyncWait_Wait");
                while (element.Displayed)
                {
                    Thread.Sleep(400);
                    element = driver.FindElementById("ReportViewerControl_AsyncWait_Wait");
                }
                Thread.Sleep(800);
            }
        }
    }
}