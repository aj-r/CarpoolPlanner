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
    public class UserController : NgController
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
                var user = context.FindUser(model.UserId, model.Password);
                if (user != null)
                {
                    AppUtils.UpdateCachedUser(user);
                    switch (user.Status)
                    {
                        case UserStatus.Disabled:
                            model.SetMessage(Resources.AccountDisabled, MessageType.Error);
                            break;
                        default:
                            // Note: allow unapproved accounts to log in, but give them limited access.
                            // Log the user in, redirect to the proper page.
                            FormsAuthentication.SetAuthCookie(model.UserId, model.RememberMe);
                            var returnUrl = Request.QueryString["ReturnUrl"];
                            return returnUrl != null ? NgRedirect(returnUrl) : NgRedirect("Trips", "Home");
                    }
                }
                else
                {
                    model.SetMessage(Resources.InvalidUsernameOrPassword, MessageType.Error);
                }
                // Clear the password
                model.Password = "";
                return Ng(model);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}
