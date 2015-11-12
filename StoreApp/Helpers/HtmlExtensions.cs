using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace StoreApp.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper, string text, string action, string controller, string icon = null, object htmlAttributes = null, string containerClass = null)
        {
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var li = new TagBuilder("li");
            string innerHtml;

            if (string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase))
            {
                li.AddCssClass("active");
            }

            if (!String.IsNullOrEmpty(containerClass))
            {
                li.AddCssClass(containerClass);
            }

            if (String.IsNullOrEmpty(icon))
            {
                innerHtml = htmlHelper.ActionLink(text, action, controller).ToHtmlString();//TODO: add htmlAttributes
            }
            else
            {
                var link = new TagBuilder("a");
                var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
                string url = urlHelper.Action(action, controller);
                link.MergeAttribute("href", url);

                if (htmlAttributes != null)
                {
                    IDictionary<string, object> attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    link.MergeAttributes(attributes);
                }

                var span = new TagBuilder("span");
                span.AddCssClass(String.Format("glyphicon {0}", icon));
                link.InnerHtml = String.Format("{0} {1}", span.ToString(), text);
                innerHtml = link.ToString();
            }

            li.InnerHtml = innerHtml;
            return MvcHtmlString.Create(li.ToString());
        }
    }
}