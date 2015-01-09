using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CarpoolPlanner
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Renders the properties of the current MVC model into a javascript object.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <returns>A javascript object that represents the current MVC model.</returns>
        public static MvcHtmlString JSModel(this HtmlHelper htmlHelper)
        {
            return JSModel(htmlHelper, htmlHelper.ViewData.Model);
        }

        /// <summary>
        /// Renders the properties of the specified MVC model into a javascript object.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <returns>A javascript object that represents the current MVC model.</returns>
        public static MvcHtmlString JSModel(this HtmlHelper htmlHelper, object model)
        {
            if (model == null)
                return new MvcHtmlString("{}");

            var serializer = JsonSerializerFactory.Current.GetSerializer();
            string json;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, model);
                json = writer.ToString();
            }
            return new MvcHtmlString(json);
        }

        /// <summary>
        /// Renders an enum type as a javascript object.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="enumType">The type of enum</param>
        /// <returns>A javascript object that contains the keys and values of the enum.</returns>
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

        /// <summary>
        /// Creates a javascript class based on the property values of the specified class type when the default constructor is called.
        /// </summary>
        /// <typeparam name="T">The type of the CLR class or struct to create in javascript.</typeparam>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <returns>A javascript class that mimics the CLR class.</returns>
        /// <remarks>
        /// The default constructor of <typeparamref name="T"/> will be used to determine the default values
        /// for properties of that type when the constructor is invoked in javascript.
        /// </remarks>
        public static MvcHtmlString JSClass<T>(this HtmlHelper htmlHelper) where T : new()
        {
            return JSClass(htmlHelper, new T());
        }

        /// <summary>
        /// Creates a javascript class based on the property values of the specified class type when the default constructor is called.
        /// </summary>
        /// <typeparam name="T">The type of the CLR class or struct to create in javascript.</typeparam>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="jsClassName">The name of the class in javascript.</param>
        /// <returns>A javascript class that mimics the CLR class.</returns>
        /// <remarks>
        /// The default constructor of <typeparamref name="T"/> will be used to determine the default values
        /// for properties of that type when the constructor is invoked in javascript.
        /// </remarks>
        public static MvcHtmlString JSClass<T>(this HtmlHelper htmlHelper, string jsClassName) where T : new()
        {
            return JSClass(htmlHelper, new T(), jsClassName);
        }

        /// <summary>
        /// Creates a javascript class based on the property values of the specified class type when the default constructor is called.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="defaultValue">An object of the type to create which contains the default values for properties of that type when the constructor is invoked in javascript.</param>
        /// <returns>A javascript class that mimics the CLR class.</returns>
        public static MvcHtmlString JSClass(this HtmlHelper htmlHelper, object defaultValue)
        {
            return JSClass(htmlHelper, defaultValue, defaultValue.GetType().Name);
        }

        /// <summary>
        /// Creates a javascript class based on the property values of the specified class type when the default constructor is called.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="defaultValue">An object of the type to create which contains the default values for properties of that type when the constructor is invoked in javascript.</param>
        /// <param name="jsClassName">The name of the class in javascript.</param>
        /// <returns>A javascript class that mimics the CLR class.</returns>
        public static MvcHtmlString JSClass(this HtmlHelper htmlHelper, object defaultValue, string jsClassName)
        {
            if (defaultValue == null)
                throw new ArgumentNullException();
            var sb = new StringBuilder();
            sb.Append(jsClassName);
            sb.Append(" = function() {};");
            sb.Append(jsClassName);
            sb.Append(".prototype = ");
            sb.Append(JSModel(htmlHelper, defaultValue));
            sb.Append(";");
            return new MvcHtmlString(sb.ToString());
        }

        private static IEnumerable<string> SplitString(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }
    }
}