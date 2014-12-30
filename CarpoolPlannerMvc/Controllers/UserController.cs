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
    [Authorize]
    public class UserController : CarpoolControllerBase
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var user = context.FindUser(model.LoginNameInput, model.Password);
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
                            FormsAuthentication.SetAuthCookie(model.LoginNameInput, model.RememberMe);
                            var returnUrl = Request.QueryString["ReturnUrl"];
                            return NgRedirect(returnUrl ?? FormsAuthentication.DefaultUrl);
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

        public ActionResult List()
        {
            var model = new UserListViewModel();
            if (AppUtils.CurrentUser.Status == UserStatus.Active)
            {
                using(var context = ApplicationDbContext.Create())
                {
                    IQueryable<User> query = context.Users;
                    if (!AppUtils.IsUserAdmin())
                        query = query.Where(u => u.Status == UserStatus.Active);
                    model.Users = query.ToList().Select(u => new UserViewModel(u)).ToList();
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new UserViewModel();
            model.User.LoginName = Request.QueryString["LoginName"];
            return View(model);
        }

        public ActionResult Manage()
        {
            return View(new UserViewModel(AppUtils.CurrentUser));
        }

        [HttpPost]
        public ActionResult Manage(UserViewModel user)
        {
            if (user.User == null)
                return Ng(user);
            if (user.User != AppUtils.CurrentUser && !AppUtils.IsUserAdmin())
            {
                Response.StatusCode = 403;
                user.SetMessage("You are not authorized to update the specified user.", MessageType.Error);
                return Ng(user);
            }
            var model = new UserListViewModel();
            using (var context = ApplicationDbContext.Create())
            {
                // TODO: save user
            }
            return Ng(model);
        }

        [HttpPost]
        public ActionResult SetPassword(string password, UserViewModel user)
        {
            if (user.User == null)
                return Ng(user);
            if (user.User != AppUtils.CurrentUser && !AppUtils.IsUserAdmin())
            {
                Response.StatusCode = 403;
                user.SetMessage("You are not authorized to update the specified user.", MessageType.Error);
                return Ng(user);
            }
            if (string.IsNullOrEmpty(password))
            {
                user.SetMessage("The specified password does not meet the requirements.", MessageType.Error);
                return Ng(user);
            }
            var model = new UserListViewModel();
            using (var context = ApplicationDbContext.Create())
            {
                // TODO: set password
            }
            return Ng(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateStatus(UserViewModel user)
        {
            if (user.User == null)
                return Ng(user);
            if (!AppUtils.IsUserAdmin())
            {
                Response.StatusCode = 403;
                user.SetMessage("You are not authorized to update the specified user's status.", MessageType.Error);
                return Ng(user);
            }
            using (var context = ApplicationDbContext.Create())
            {
                var serverUser = context.Users.Find(user.User.Id);
                if (serverUser == null)
                {
                    Response.StatusCode = 400;
                    user.SetMessage("Specified user does not exist.", MessageType.Error);
                    return Ng(user);
                }
                serverUser.Status = user.User.Status;
                serverUser.IsAdmin = user.User.IsAdmin;
                context.SaveChanges();
                user.User = serverUser;
                user.SetMessage("Successful", MessageType.Success);
                return Ng(user);
            }
        }
    }
}
