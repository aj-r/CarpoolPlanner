using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CarpoolPlanner
{
    /// <summary>
    /// Represents a class that is used to send JSON-formatted content (serialized with Newtonsoft.JSON) to the response.
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new IsoDateTimeConverter(), new KeyedCollectionConverter() },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var serializedObject = JsonConvert.SerializeObject(Data, settings);
            response.Write(serializedObject);
        }
    }
}