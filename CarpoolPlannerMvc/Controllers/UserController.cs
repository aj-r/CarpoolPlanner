using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CarpoolPlanner.Model;
using CarpoolPlanner.ViewModel;

namespace CarpoolPlanner.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            // TODO: test throwing an exception - make sure some kind of error is displayed to the user.
            using (var context = ApplicationDbContext.Create())
            {
                var user = context.FindUser(model.UserName, model.Password);
                if (user != null)
                {
                    AppUtils.UpdateCachedUser(user);
                    switch (user.Status)
                    {
                        case UserStatus.Disabled:
                            model.SetMessage("Your account has has been disabled.", MessageType.Error);
                            break;
                        default:
                            // Note: allow unapproved accounts to log in, but give them limited access.
                            // Log the user in, redirect to the proper page.
                            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                            return AjaxRedirect("Trips", "Home");
                    }
                }
                else
                {
                    model.SetMessage("Invalid username or password.", MessageType.Error);
                }
                // Clear the password
                model.Password = "";
                return Json(model);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);
        }

        // TODO: move to some kind of controller base class
        /// <summary>
        /// Redirects an ajax request to the specified action.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns></returns>
        protected JsonResult AjaxRedirect(string actionName, string controllerName)
        {
            return AjaxRedirect(Url.Action(actionName, controllerName));
        }

        // TODO: move to some kind of controller base class
        /// <summary>
        /// Redirects an ajax request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to redirect to.</param>
        /// <returns></returns>
        protected JsonResult AjaxRedirect(string url)
        {
            return Json(new { redirectUrl = url });
        }
    }
}
