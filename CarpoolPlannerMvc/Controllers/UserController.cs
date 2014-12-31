using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CarpoolPlanner.Model;
using CarpoolPlanner.ViewModel;
using log4net;

namespace CarpoolPlanner.Controllers
{
    [Authorize]
    public class UserController : CarpoolControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(User));

        private const int saltSize = 24;
        private const int hashSize = 24;
        private const int defaultIterationCount = 1000;

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
                var user = context.Users.FirstOrDefault(u => u.LoginName == model.LoginNameInput);
                if (IsPasswordCorrect(user, model.Password))
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
            return View(new CreateUserViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(CreateUserViewModel model)
        {
            if (model == null)
            {
                Response.StatusCode = 400;
                model.SetMessage("No model specified.", MessageType.Error);
                model.Password = "";
                log.Warn(model.Message);
                return Ng(model);
            }
            if (model.User == null)
            {
                Response.StatusCode = 400;
                model.SetMessage("User is required.", MessageType.Error);
                model.Password = "";
                log.Warn(model.Message);
                return Ng(model);
            }
            if (model.User.LoginName == null)
            {
                Response.StatusCode = 400;
                model.SetMessage("Login name is required.", MessageType.Error);
                model.Password = "";
                log.Warn(model.Message);
                return Ng(model);
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                Response.StatusCode = 400;
                model.SetMessage("Password is required.", MessageType.Error);
                model.Password = "";
                log.Warn(model.Message);
                return Ng(model);
            }
            SetPassword(model.User, model.Password);
            model.User.Status = UserStatus.Unapproved;
            using (var context = ApplicationDbContext.Create())
            {
                if (context.Users.Any(u => u.LoginName == model.User.LoginName))
                {
                    model.SetMessage(string.Concat("User ID '", model.User.LoginName, "' already exists."), MessageType.Error);
                    model.Password = "";
                    log.Warn(model.Message);
                    return Ng(model);
                }
                context.Users.Add(model.User);
                context.SaveChanges();
            }
            // Take the user to the trips page because unapproved users won't have access to the default page anyways.
            // This allows users to enrol in trips while they wait to be approved.
            return NgRedirect(Url.Action("Index", "Trips"));
        }

        public ActionResult Manage()
        {
            return View(new UserViewModel(AppUtils.CurrentUser));
        }

        [HttpPost]
        public ActionResult Manage(UserViewModel model)
        {
            if (model.User == null)
                return Ng(model);
            if (AppUtils.CurrentUser == null || (model.User.Id != AppUtils.CurrentUser.Id && !AppUtils.IsUserAdmin()))
            {
                Response.StatusCode = 403;
                model.SetMessage("You are not authorized to update the specified user.", MessageType.Error);
                return Ng(model);
            }
            using (var context = ApplicationDbContext.Create())
            {
                var serverUser = context.Users.Find(model.User.Id);
                if (serverUser == null)
                {
                    Response.StatusCode = 400;
                    model.SetMessage("User not found.", MessageType.Error);
                    return Ng(model);
                }
                serverUser.Name = model.User.Name;
                serverUser.CommuteMethod = model.User.CommuteMethod;
                serverUser.CanDriveIfNeeded = model.User.CanDriveIfNeeded;
                serverUser.Seats = model.User.Seats;
                serverUser.Email = model.User.Email;
                serverUser.EmailNotify = model.User.EmailNotify;
                serverUser.EmailVisible = model.User.EmailVisible;
                serverUser.Phone = model.User.Phone;
                serverUser.PhoneNotify = model.User.PhoneNotify;
                serverUser.PhoneVisible = model.User.PhoneVisible;
                context.SaveChanges();
                model.User = serverUser;
                AppUtils.UpdateCachedUser(serverUser);
            }
            model.SetMessage("Saved successfully.", MessageType.Success);
            return Ng(model);
        }

        [HttpPost]
        public ActionResult SetPassword(SetPasswordViewModel model)
        {
            if (AppUtils.CurrentUser == null || (model.UserId != AppUtils.CurrentUser.Id && !AppUtils.IsUserAdmin()))
            {
                Response.StatusCode = 403;
                model.SetMessage("You are not authorized to set the password for the specified user.", MessageType.Error);
                model.OldPassword = "";
                model.NewPassword = "";
                return Ng(model);
            }
            if (string.IsNullOrEmpty(model.NewPassword))
            {
                model.SetMessage("The specified password does not meet the requirements.", MessageType.Error);
                model.OldPassword = "";
                model.NewPassword = "";
                return Ng(model);
            }
            using (var context = ApplicationDbContext.Create())
            {
                var user = context.Users.Find(model.UserId);
                if (!IsPasswordCorrect(user, model.OldPassword))
                {
                    model.SetMessage("Current password is incorrect.", MessageType.Error);
                    model.OldPassword = "";
                    model.NewPassword = "";
                    return Ng(model);
                }
                SetPassword(user, model.NewPassword);
                context.SaveChanges();
                AppUtils.UpdateCachedUser(user);
            }
            model.OldPassword = "";
            model.NewPassword = "";
            model.SetMessage("Changed password successfully.", MessageType.Success);
            return Ng(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateStatus(UserViewModel model)
        {
            if (model.User == null)
                return Ng(model);
            if (!AppUtils.IsUserAdmin())
            {
                Response.StatusCode = 403;
                model.SetMessage("You are not authorized to update the specified user's status.", MessageType.Error);
                return Ng(model);
            }
            using (var context = ApplicationDbContext.Create())
            {
                var serverUser = context.Users.Find(model.User.Id);
                if (serverUser == null)
                {
                    Response.StatusCode = 400;
                    model.SetMessage("Specified user does not exist.", MessageType.Error);
                    return Ng(model);
                }
                serverUser.Status = model.User.Status;
                serverUser.IsAdmin = model.User.IsAdmin;
                context.SaveChanges();
                model.User = serverUser;
                AppUtils.UpdateCachedUser(serverUser);
            }
            model.SetMessage("Successful", MessageType.Success);
            return Ng(model);
        }

        /// <summary>
        /// Checks whether the specified plain-text password is the correct password for the current user.
        /// </summary>
        /// <param name="password">The plain-text password to check.</param>
        private static bool IsPasswordCorrect(User user, string password)
        {
            if (password == null)
                password = "";
            if (user != null && user.Password != null && user.Salt != null)
            {
                var hash = Crypto.PBKDF2(password, user.Salt, user.Iterations, hashSize);
                return Crypto.SlowEquals(hash, user.Password);
            }
            else
            {
                // If user does not exist, perform operations that should take the same amount of time as verifying the password.
                // This prevents attacks that use response timing to determine whether a user exists.
                var buffer = new byte[saltSize];
                var hash = Crypto.PBKDF2(password, buffer, defaultIterationCount, hashSize);
                Crypto.SlowEquals(hash, buffer);
                return false;
            }
        }

        /// <summary>
        /// Sets the current user's password.
        /// </summary>
        /// <param name="password">The plain-text password.</param>
        private static void SetPassword(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (password == null)
                throw new ArgumentNullException("password");
            var rng = new RNGCryptoServiceProvider();
            user.Salt = new byte[saltSize];
            rng.GetBytes(user.Salt);
            user.Iterations = defaultIterationCount;
            user.Password = Crypto.PBKDF2(password, user.Salt, user.Iterations, hashSize);
            log.Info(string.Concat("User ", user.Id, " changed his/her password"));
        }
    }
}
