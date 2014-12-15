using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CarpoolPlanner
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.RadioButtonListFor(expression, null);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var sb = new StringBuilder();
            var propertyType = typeof(TProperty);
            if (!propertyType.IsEnum)
                throw new NotSupportedException("Can only generate radio button lists for enum types.");
            foreach (var value in Enum.GetValues(propertyType))
            {
                // TODO: apply htmlAttributes
                sb.Append(htmlHelper.RadioButtonFor(expression, value).ToString());
                var displayAttr = propertyType.GetMember(value.ToString())
                    .Select(m => (DisplayAttribute)Attribute.GetCustomAttribute(m, typeof(DisplayAttribute), false))
                    .FirstOrDefault();
                if (displayAttr != null && displayAttr.Name != null)
                    sb.Append(htmlHelper.Label(displayAttr.Name).ToString());
                else
                    sb.Append(htmlHelper.Label(value.ToString()).ToString());
            }
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}