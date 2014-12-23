using System;
using System.Web;
using System.Web.Mvc;

namespace CarpoolPlanner.Controllers
{
    public class ControllerBase : NgController
    {
        /// <summary>
        /// Gets a result that redirects the client to the login page.
        /// </summary>
        protected RedirectToRouteResult RedirectToLogin()
        {
            return RedirectToAction("Login", "User", new { ReturnUrl = Request.Path });
        }
    }
}