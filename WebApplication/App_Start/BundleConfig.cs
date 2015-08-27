using System.Web.Optimization;
using System.Web.Optimization.React;

namespace SKBKontur.Treller.WebApplication
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js/polyfills")
                .Include("~/Scripts/es5-shim.js")
                .Include("~/Scripts/es5-sham.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/base-libraries")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/react/react-with-addons-{version}.js")
                .Include("~/Scripts/react/JSXTransformer-{version}.js")
                .Include("~/Scripts/Colorbox/jquery.colorbox.js")
                .IncludeDirectory("~/Content/Scripts/Shared", "*.js"));

            bundles.Add(new JsxBundle("~/bunsles/jsx/tasklist")
                .IncludeDirectory("~/Content/Scripts/Layout", "*.jsx")
                .IncludeDirectory("~/Content/Scripts/TaskList", "*.jsx"));

            
            
            bundles.Add(
                new StyleBundle("~/bundles/layout")
                .Include("~/Content/normalize.css")
                .Include("~/Content/Bootstrap/bootstrap.css")
                .Include("~/Content/Bootstrap/bootstrap-theme.css")
                .Include("~/Content/Colorbox/colorbox.css")
                .Include("~/Content/site.css")
                .IncludeDirectory("~/Content/Layout/", "*.css"));
            
            bundles.Add(
                new StyleBundle("~/bundles/css/tasklist")
                .IncludeDirectory("~/Content/Shared", "*.css")
                .IncludeDirectory("~/Content/TaskList", "*.css")
                .IncludeDirectory("~/Content/TaskInfo", "*.css"));

            bundles.Add(new StyleBundle("~/bundles/css/News")
                .IncludeDirectory("~/Content/News", "*.css"));
        }
    }
}