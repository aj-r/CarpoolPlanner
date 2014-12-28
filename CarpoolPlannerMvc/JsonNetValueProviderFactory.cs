using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace CarpoolPlanner
{
    public class JsonNetValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                return null;

            var streamReader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            var jsonReader = new JsonTextReader(streamReader);
            if (!jsonReader.Read())
                return null;
            var jToken = JToken.Load(jsonReader);
            return new JsonNetValueProvider(jToken);
        }
    }

    public class JsonNetValueProvider : IValueProvider
    {
        private JToken rootToken;

        /// <summary>
        /// Creates a new JsonNetValueProviderinstance from the specified JToken.
        /// </summary>
        /// <param name="token">The root JToken that represents the entire object or array.</param>
        public JsonNetValueProvider(JToken token)
        {
            rootToken = token;
        }

        #region IValueProvider Members

        public bool ContainsPrefix(string prefix)
        {
            var token = GetToken(prefix);
            if (token == null && prefix.EndsWith(".index"))
            {
                token = GetToken(prefix.Remove(prefix.Length - 6));
            }
            return token != null;
        }

        public ValueProviderResult GetValue(string key)
        {
            var token = GetToken(key);
            ValueProviderResult result = null;
            if (token != null)
            {
                // Only return JValue values; no JObjects or JArrays because the ModelBinder doesn't know how to handle them.
                var jvalue = token as JValue;
                if (jvalue != null)
                {
                    var value = jvalue.Value;
                    // Required for enums serialized as numbers: convert long to int if it is within range.
                    if (value is long)
                    {
                        var longValue = (long)value;
                        if (longValue >= int.MinValue && longValue <= int.MaxValue)
                            value = (int)longValue;
                    }
                    result = new ValueProviderResult(value, token.ToString(), CultureInfo.CurrentCulture);
                }
            }
            else if (key.EndsWith(".index"))
            {
                var keyName = key.Remove(key.Length - 6);
                token = GetToken(keyName);
                // Return all propery names of the specified JObject
                string[] indexes = null;
                var obj = token as JObject;
                if (obj != null)
                {
                    indexes = obj.Properties().Select(p => p.Name).ToArray();
                    result = new ValueProviderResult(indexes, token.ToString(), CultureInfo.CurrentCulture);
                }
            }
            return result;
        }

        #endregion

        private static readonly Regex fixPath = new Regex(@"\[([^\]]*[^0-9\]]+[^\]]*)\]", RegexOptions.Compiled);

        private JToken GetToken(string path)
        {
            try
            {
                // Replace non-numerical indexers (e.g. obj[propertyName]) with property accessors (e.g. obj.propertyName)
                path = fixPath.Replace(path, ".$1");
                return rootToken.SelectToken(path, false);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}