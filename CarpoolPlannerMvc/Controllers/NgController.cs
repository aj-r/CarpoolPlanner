using System;
using System.Web.Mvc;

namespace CarpoolPlanner.Controllers
{
    public class NgController : Controller
    {
        /// <summary>
        /// Creates a JsonResult that tells angular to update the model.
        /// </summary>
        /// <param name="model">The updated model.</param>
        /// <returns></returns>
        protected JsonResult Ng(object model)
        {
            return Ng(model, null);
        }

        /// <summary>
        /// Creates a JsonResult that tells angular to update the model and user id.
        /// </summary>
        /// <param name="model">The updated model.</param>
        /// <param name="userId">The new user ID.</param>
        /// <returns></returns>
        protected JsonResult Ng(object model, object userId)
        {
            return Json(new NgResultData { model = model, userId = userId });
        }

        /// <summary>
        /// Redirects an ajax request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns></returns>
        protected JsonResult NgRedirect(string url)
        {
            return Json(new NgResultData { redirectUrl = url });
        }
    }
}