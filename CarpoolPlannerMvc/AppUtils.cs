using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using CarpoolPlanner.Model;

namespace CarpoolPlanner
{
    public static class AppUtils
    {
        public const string AppName = "Climbers Carpooling";
        public const string DateFormat = "dd-MMM-yyyy";

        private static readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();

        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current.User == null)
                    return null;
                var loginName = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(loginName))
                    return null;
                var user = Users.GetOrAdd(loginName.ToLowerInvariant(), key =>
                {
                    using (var context = ApplicationDbContext.Create())
                    {
                        return context.Users.FirstOrDefault(u => u.LoginName == key);
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
            Users.AddOrUpdate(user.LoginName.ToLowerInvariant(), user, (id, u) => user);
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