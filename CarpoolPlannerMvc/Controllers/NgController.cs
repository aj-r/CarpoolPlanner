using System;
using System.Text;
using System.Web.Mvc;

namespace CarpoolPlanner.Controllers
{
    public class NgController : Controller
    {
        /// <summary>
        /// Creates a JsonNetResult that tells angular to update the model.
        /// </summary>
        /// <param name="model">The updated model.</param>
        /// <returns></returns>
        protected JsonNetResult Ng(object model)
        {
            return Ng(model, null);
        }

        /// <summary>
        /// Creates a JsonNetResult that tells angular to update the model and user id.
        /// </summary>
        /// <param name="model">The updated model.</param>
        /// <param name="userId">The new user ID.</param>
        /// <returns></returns>
        protected JsonNetResult Ng(object model, object userId)
        {
            return JsonNet(new NgResultData { model = model, userId = userId });
        }

        /// <summary>
        /// Creates a JsonNetResult that instructs Angular to change the location to the specified URL.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns></returns>
        protected JsonNetResult NgRedirect(string url)
        {
            return JsonNet(new NgResultData { redirectUrl = url });
        }

        /// <summary>
        /// Creates a JsonNetResult that serializes an object to JSON using Newtonsoft.Json.
        /// </summary>
        /// <returns></returns>
        protected JsonNetResult JsonNet(object data)
        {
            return JsonNet(data, null);
        }

        /// <summary>
        /// Creates a JsonNetResult that serializes an object to JSON using Newtonsoft.Json.
        /// </summary>
        /// <returns></returns>
        protected JsonNetResult JsonNet(object data, string contentType)
        {
            return JsonNet(data, contentType, null);
        }

        /// <summary>
        /// Creates a JsonNetResult that serializes an object to JSON using Newtonsoft.Json.
        /// </summary>
        /// <returns></returns>
        protected virtual JsonNetResult JsonNet(object data, string contentType, Encoding contentEncoding)
        {
            return JsonNet(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// Creates a JsonNetResult that serializes an object to JSON using Newtonsoft.Json.
        /// </summary>
        /// <returns></returns>
        protected JsonNetResult JsonNet(object data, JsonRequestBehavior behaviour)
        {
            return JsonNet(data, null, behaviour);
        }

        /// <summary>
        /// Creates a JsonNetResult that serializes an object to JSON using Newtonsoft.Json.
        /// </summary>
        /// <returns></returns>
        protected JsonNetResult JsonNet(object data, string contentType, JsonRequestBehavior behaviour)
        {
            return JsonNet(data, contentType, null, behaviour);
        }

        /// <summary>
        /// Creates a JsonNetResult that serializes an object to JSON using Newtonsoft.Json.
        /// </summary>
        /// <returns></returns>
        protected virtual JsonNetResult JsonNet(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behaviour)
        {
            return new JsonNetResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behaviour };
        }
    }
}