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

        public static string CurrentUserId
        {
            get { return HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : null; }
        }

        public static User CurrentUser
        {
            get
            {
                var userId = CurrentUserId;
                if (string.IsNullOrEmpty(userId))
                    return null;
                var user = Users.GetOrAdd(userId.ToLowerInvariant(), id =>
                {
                    using (var context = ApplicationDbContext.Create())
                    {
                        return context.Users.Find(id);
                    }
                });
                return user;
            }
        }

        public static bool IsUserAuthenticated()
        {
            return !string.IsNullOrEmpty(CurrentUserId);
        }

        public static bool IsUserStatus(UserStatus status)
        {
            var user = CurrentUser;
            return user != null && user.Status == status;
        }

        public static bool IsUserAdmin()
        {
            var user = CurrentUser;
            return user != null && user.IsAdmin;
        }

        public static void UpdateCachedUser(User user)
        {
            Users.AddOrUpdate(user.Id.ToLowerInvariant(), user, (id, u) => user);
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