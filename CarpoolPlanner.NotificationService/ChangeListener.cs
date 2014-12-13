using System;
using System.Collections.Generic;
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
            var tripInstanceId = int.Parse(args[0]);
            var tripRecurrenceId = int.Parse(args[1]);
            using (var context = ApplicationDbContext.Create())
            {
                var tripInstance = context.TripInstances.Find(tripInstanceId);
                NotificationManager.GetInstance().SetNextNotificationTimes(tripInstance, tripRecurrenceId);
            }
            return "";
        }

        [RestMethod(Name = "/notify-ti", ParamCount = 2, ContentType = "application/json")]
        public string NotifyTripInstance(string[] args)
        {
            var tripInstanceId = int.Parse(args[0]);
            var tripRecurrenceId = int.Parse(args[1]);
            using (var context = ApplicationDbContext.Create())
            {
                var tripInstance = context.TripInstances.Find(tripInstanceId);
                // TODO: re-notify users who are attending this instance
            }
            return "";
        }
    }
}
