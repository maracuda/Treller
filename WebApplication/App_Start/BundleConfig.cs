using System.Web.Optimization;

namespace SKBKontur.Treller.WebApplication.App_Start
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/base-libraries")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/Colorbox/jquery.colorbox.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            
            bundles.Add(
                new StyleBundle("~/bundles/layout")
                .Include("~/Content/normalize.css")
                .Include("~/Content/Bootstrap/bootstrap.css")
                .Include("~/Content/Bootstrap/bootstrap-theme.css")
                .Include("~/Content/Colorbox/colorbox.css")
                .Include("~/Content/site.css")
                .Include("~/fonts/font-awesome/css/font-awesome.css")
                .IncludeDirectory("~/Content/Layout/", "*.css"));

            bundles.Add(
                new StyleBundle("~/bundles/tasklist")
                .IncludeDirectory("~/Content/TaskList", "*.css")
                .IncludeDirectory("~/Content/TaskInfo", "*.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}