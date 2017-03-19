using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Linq;
using System.Web.Mvc;

namespace SKBKontur.Treller.WebApplication.Implementation.Webpack
{
    public class Webpack
    {
        private const string resourcePath = "/Content/bundle/";
        private const string scriptSourceExtension = "js";
        private const string stylesSourceExtension = "css";

        public static IHtmlString Scripts(params string[] entryPoints)
        {
            var urlsByEntryPoints = SelectResourceUrls(entryPoints, scriptSourceExtension);
            var enumerable = urlsByEntryPoints.Select(RenderScriptTag);
            return CreateHtmlResult(enumerable);
        }

        public static IHtmlString Styles(params string[] entryPoints)
        {
            var urlsByEntryPoints = SelectResourceUrls(entryPoints, stylesSourceExtension);
            var enumerable = urlsByEntryPoints.Select(RenderLinkTag);
            return CreateHtmlResult(enumerable);
        }

        private static IHtmlString CreateHtmlResult(IEnumerable enumerable)
        {
            var result = new StringBuilder();
            foreach (var tag in enumerable)
            {
                result.Append(tag);
                result.Append(Environment.NewLine);
            }

            return new HtmlString(result.ToString());
        }

        private static string[] SelectResourceUrls(string[] entryPoints, string extension)
        {
            return entryPoints
                .Select(entryPoint => Path.Combine(resourcePath, string.Format("{0}.{1}", entryPoint, extension)))
                .ToArray();
        }

        private static string RenderScriptTag(string url)
        {
            var scriptTag = new TagBuilder("script");
            scriptTag.Attributes.Add("src", url);
            scriptTag.Attributes.Add("charset", "UTF-8");
            return scriptTag.ToString(TagRenderMode.Normal);
        }

        private static string RenderLinkTag(string url)
        {
            var linkTag = new TagBuilder("link");
            linkTag.Attributes.Add("rel", "stylesheet");
            linkTag.Attributes.Add("href", url);
            return linkTag.ToString(TagRenderMode.SelfClosing);
        }
    }
}