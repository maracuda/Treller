using System.Web;
using System.Web.Mvc;

namespace WebApplication.Implementation.React
{
    public class HtmlReact
    {
        public static IHtmlString Component<T>(string componentName, T props)
        {
            var component = ReactComponent.Instance(componentName, props);

            var htmlTag = new TagBuilder("div");
            htmlTag.Attributes.Add("id", component.GetId());

            return MvcHtmlString.Create(htmlTag.ToString());
        }

        public static IHtmlString Init()
        {
            var component = ReactComponent.Instance();
            var scriptTag = new TagBuilder("script")
            {
                InnerHtml = component.RenderJavaScript()
            };

            return MvcHtmlString.Create(scriptTag.ToString(TagRenderMode.Normal));
        }
    }
}