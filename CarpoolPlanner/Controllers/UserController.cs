using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CarpoolPlanner.Model;
using CarpoolPlanner.ViewModel;
using log4net;

namespace CarpoolPlanner.Controllers
{
    [Authorize]
    public class UserController : CarpoolControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserController));

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
                var user = context.Users.FirstOrDefault(u => u.Email == model.Email);
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
                            FormsAuthentication.SetAuthCookie(user.Id.ToString(), model.RememberMe);
                            var returnUrl = Request.QueryString["ReturnUrl"];
                            return NgRedirect(returnUrl ?? FormsAuthentication.DefaultUrl);
                    }
                }
                else
                {
                    log.Warn(string.Concat("Incorrect username/password for login '", model.Email, "'"));
                    model.SetMessage(Resources.InvalidUsernameOrPassword, MessageType.Error);
                }
                // Clear the password
                model.Password = "";
                return Ng(model);
            }
        }

        // Allow anonymous access to Logout, otherwise issues happen when the use clicks the Logout button after their session times out.
        [AllowAnonymous] 
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl + "?reason=logout");
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
                    model.Users = query.OrderBy(u => u.Name).ToList().Select(u => new UserViewModel(u)).ToList();
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
            if (string.IsNullOrEmpty(model.User.Email))
            {
                Response.StatusCode = 400;
                model.SetMessage("E-mail is required.", MessageType.Error);
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
            // Override any bad data the client may have sent.
            model.User.IsAdmin = false;
            model.User.Status = UserStatus.Unapproved;
            model.User.LastTextMessageId = null;
            using (var context = ApplicationDbContext.Create())
            {
                if (context.Users.Any(u => u.Email == model.User.Email))
                {
                    model.SetMessage(string.Concat("E-mail '", model.User.Email, "' is already used by another user."), MessageType.Error);
                    model.Password = "";
                    log.Warn(model.Message);
                    return Ng(model);
                }
                context.Users.Add(model.User);
                context.SaveChanges();
            }
            var client = new NotificationServiceClient();
            client.UserRegister(model.User.Id);

            // Automatically log in.
            FormsAuthentication.SetAuthCookie(model.User.Id.ToString(), false);
            // Take the user to the trips page because unapproved users won't have access to the default page anyways.
            // This allows users to enrol in trips while they wait to be approved.
            return NgRedirect(Url.Action("Index", "Trips", new RouteValueDictionary { { "justRegistered", "true" } }));
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
                serverUser.Email = model.User.Email;
                serverUser.EmailNotify = model.User.EmailNotify;
                serverUser.EmailVisible = model.User.EmailVisible;
                serverUser.Phone = model.User.Phone;
                serverUser.PhoneNotify = model.User.PhoneNotify;
                serverUser.PhoneVisible = model.User.PhoneVisible;
                if (serverUser.CommuteMethod != model.User.CommuteMethod || serverUser.CanDriveIfNeeded != model.User.CanDriveIfNeeded
                    || serverUser.Seats != model.User.Seats)
                {
                    // If there are any UserTripInstances that match the current settings, automatically update them
                    var minDate = DateTime.UtcNow - ApplicationDbContext.TripInstanceRemovalDelay;
                    var query = context.UserTripInstances.Include(uti => uti.TripInstance).Where(
                        uti => uti.UserId == serverUser.Id
                            && uti.FinalNotificationTime == null
                            && uti.TripInstance.Date > minDate
                            && uti.CommuteMethod == serverUser.CommuteMethod
                            && uti.CanDriveIfNeeded == serverUser.CanDriveIfNeeded
                            && uti.Seats == serverUser.Seats);
                    foreach (var userTripInstnace in query)
                    {
                        userTripInstnace.CommuteMethod = model.User.CommuteMethod;
                        userTripInstnace.CanDriveIfNeeded = model.User.CanDriveIfNeeded;
                        userTripInstnace.Seats = model.User.Seats;
                    }
                    // Now update the user
                    serverUser.CommuteMethod = model.User.CommuteMethod;
                    serverUser.CanDriveIfNeeded = model.User.CanDriveIfNeeded;
                    serverUser.Seats = model.User.Seats;
                }
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
                log.Info(string.Concat("User ", + user.Id, " changed his/her password"));
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
            var updateNotifications = false;
            var tripInstancesToUpdate = new List<long>();
            using (var context = ApplicationDbContext.Create())
            {
                var serverUser = context.Users.Find(model.User.Id);
                if (serverUser == null)
                {
                    Response.StatusCode = 400;
                    model.SetMessage("Specified user does not exist.", MessageType.Error);
                    return Ng(model);
                }
                if (serverUser.Status != model.User.Status)
                {
                    serverUser.Status = model.User.Status;
                    updateNotifications = true;
                    foreach (var userTrip in context.UserTrips.Where(ut => ut.User.Id == serverUser.Id && ut.Attending).Include(ut => ut.Instances))
                    {
                        foreach (var userTripInstance in userTrip.Instances.Where(uti => uti.Attending == null))
                        {
                            tripInstancesToUpdate.Add(userTripInstance.TripInstanceId);
                        }
                    }
                }
                serverUser.IsAdmin = model.User.IsAdmin;
                context.SaveChanges();
                model.User = serverUser;
                AppUtils.UpdateCachedUser(serverUser);
            }
            if (updateNotifications)
            {
                var client = new NotificationServiceClient();
                foreach (var id in tripInstancesToUpdate)
                {
                    client.UpdateTripInstance(id);
                }
            }
            model.SetMessage("Successful", MessageType.Success);
            return Ng(model);
        }

        /// <summary>
        /// Checks whether the specified plain-text password is the correct password for the current user.
        /// </summary>
        /// <param name="password">The plain-text password to check.</param>
        public static bool IsPasswordCorrect(User user, string password)
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
        public static void SetPassword(User user, string password)
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

        /// <summary>
        /// Gets a value that indicates whether the current user has access to modify the specified user.
        /// </summary>
        /// <param name="user">The user being modified.</param>
        /// <returns></returns>
        public static bool CanCurrentUserModify(User user)
        {
            if (user == null || user.IsNew())
                return true;
            var currentUser = AppUtils.CurrentUser;
            return currentUser != null && (currentUser.IsAdmin || currentUser.Id == user.Id);
        }
    }
}
