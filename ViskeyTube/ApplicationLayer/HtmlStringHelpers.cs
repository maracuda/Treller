using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace ViskeyTube.ApplicationLayer
{
    public static class HtmlStringHelpers
    {
        public static string FromHtml(this string source)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(source);

            return HttpUtility.HtmlDecode(Regex.Replace(htmlDocument.ParsedText, "<.*?>", "\r\n")).Replace("\r\n\r\n", "\r\n");

            var lines = GetLines(htmlDocument.DocumentNode).ToArray();
            return string.Join("\r\n", lines).Replace("&quot;","\"");
        }

        private static IEnumerable<string> GetLines(HtmlNode node)
        {
            yield return node.InnerText;
            foreach (var childNode in node.ChildNodes)
            {
                foreach (var line in GetLines(childNode))
                {
                    yield return line;
                }
            }
        }
    }
}