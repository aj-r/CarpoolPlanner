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

        protected JsonResult AjaxRedirect(string actionName, string controllerName)
        {
            return Json(new { redirectUrl = Url.Action(actionName, controllerName) });
        }
    }
}
