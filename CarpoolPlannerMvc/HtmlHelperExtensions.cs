using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

        /// <summary>
        /// Renders the properties of the current MVC model into a javascript object.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="withSemicolon">True to include a semicolon at the end of the output, otherwise false.</param>
        /// <returns>A javascript object that represents the current MVC model.</returns>
        public static MvcHtmlString JSModel(this HtmlHelper htmlHelper)
        {
            var model = htmlHelper.ViewData.Model;
            if (model == null)
                return new MvcHtmlString("");

            var serializer = JsonSerializerFactory.Current.GetSerializer();
            string json;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, model);
                json = writer.ToString();
            }
            return new MvcHtmlString("JSON.parse('" + json + "')");
        }

        /// <summary>
        /// Renders an enum type as a javascript object.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <returns>A javascript object that represents the current MVC model.</returns>
        public static MvcHtmlString JSEnum(this HtmlHelper htmlHelper, Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("enumType must be an enum type.", "enumType");
            var sb = new StringBuilder();
            sb.Append('{');
            bool separator = false;
            foreach (var value in enumType.GetEnumValues())
            {
                if (separator)
                    sb.Append(',');
                sb.Append(value);
                sb.Append(':');
                sb.Append((int)value);
                separator = true;
            }
            sb.Append('}');
            return new MvcHtmlString(sb.ToString());
        }
    }
}