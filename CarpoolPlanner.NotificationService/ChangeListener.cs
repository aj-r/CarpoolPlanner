using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarpoolPlanner.Model;
using Rest.Server;

namespace CarpoolPlanner.NotificationService
{
    public class ChangeListener : RestServer
    {
        public ChangeListener()
            : base(23122) // Note that this port should NOT be opened on the server because it is only used for inter-process communication.
        { }

        [RestMethod(Name = "/update-ti", ParamCount = 2, ContentType = "application/json")]
        public string UpdateTripInstance(string[] args)
        {
            if (Program.Verbose)
                Console.WriteLine("Received update-ti message (" + args[0] + ")");
            var tripInstanceId = long.Parse(args[0]);
            var tripRecurrenceId = long.Parse(args[1]);
            TripInstance tripInstance;
            using (var context = ApplicationDbContext.Create())
            {
                tripInstance = context.TripInstances.Find(tripInstanceId);
            }
            if (tripInstance == null)
                return "Trip instance not found";
            if (Program.Verbose)
                Console.WriteLine("Updating notifications");
            NotificationManager.GetInstance().SetNextNotificationTimes(tripInstance, tripRecurrenceId);
            return "Success";
        }

        [RestMethod(Name = "/notify-ti", ParamCount = 1, ContentType = "application/json")]
        public string NotifyTripInstance(string[] args)
        {
            if (Program.Verbose)
                Console.WriteLine("Received notify-ti message (" + args[0] + ")");
            var tripInstanceId = long.Parse(args[0]);
            TripInstance tripInstance;
            using (var context = ApplicationDbContext.Create())
            {
                tripInstance = context.TripInstances.Include(ti => ti.UserTripInstances).FirstOrDefault(ti => ti.Id == tripInstanceId);
            }
            if (tripInstance == null)
                return "Trip instance not found";
            // Only re-send notifications if drivers have been picked (i.e. the final notification has been sent)
            if (tripInstance.DriversPicked)
            {
                if (Program.Verbose)
                    Console.WriteLine("Re-notifying");
                NotificationManager.GetInstance().SendFinalNotification(tripInstanceId, true);
            }
            return "Success";
        }

        [RestMethod(Name = "/user-register", ParamCount = 1, ContentType = "application/json")]
        public string UserRegister(string[] args)
        {
            if (Program.Verbose)
                Console.WriteLine("Received user-register message (" + args[0] + ")");
            var userId = long.Parse(args[0]);
            User user;
            using (var context = ApplicationDbContext.Create())
            {
                user = context.Users.Find(userId);
                if (user == null)
                    return "User not found";
                var manager = NotificationManager.GetInstance();
                // Notify all admins that a user registered to they can approve/disable the new user.
                var message = string.Concat("A new user has registered (", user.Email, "). Go to https://climbing.pororeplays.com/User/List to approve them.");
                foreach (var admin in context.Users.Where(u => u.IsAdmin && u.Status == UserStatus.Active))
                {
                    manager.SendNotification(admin, "New user registered", message).Wait();
                }
            }
            return "Success";
        }
    }
}
