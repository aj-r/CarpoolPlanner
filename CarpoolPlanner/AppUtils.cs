using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using CarpoolPlanner.Model;

namespace CarpoolPlanner
{
    public static class AppUtils
    {
        public static readonly string AppName = ConfigurationManager.AppSettings["AppName"];

        private static readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();

        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.User == null)
                    return null;
                var userIdString = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(userIdString))
                    return null;
                var user = Users.GetOrAdd(userIdString, key =>
                {
                    using (var context = ApplicationDbContext.Create())
                    {
                        long userId;
                        if (long.TryParse(userIdString, out userId))
                            return context.Users.Find(userId);
                        return new User { Status = UserStatus.Unapproved };
                    }
                });
                return user;
            }
        }

        public static bool IsUserAuthenticated()
        {
            return CurrentUser != null;
        }

        public static bool IsUserStatus(UserStatus status)
        {
            var user = CurrentUser;
            return user != null ? user.Status == status : status == UserStatus.Disabled;
        }

        public static bool IsUserAdmin()
        {
            var user = CurrentUser;
            return user != null && user.IsAdmin;
        }

        public static void UpdateCachedUser(User user)
        {
            Users.AddOrUpdate(user.Id.ToString(), user, (id, u) => user);
        }

        /// <summary>
        /// Gets the application version as it appears in the AssemblyInfo file.
        /// </summary>
        public static Version Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }
    }
}