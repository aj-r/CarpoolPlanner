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
            var tripInstanceId = long.Parse(args[0]);
            var tripRecurrenceId = long.Parse(args[1]);
            TripInstance tripInstance;
            using (var context = ApplicationDbContext.Create())
            {
                tripInstance = context.TripInstances.Find(tripInstanceId);
            }
            if (tripInstance == null)
                return "";
            NotificationManager.GetInstance().SetNextNotificationTimes(tripInstance, tripRecurrenceId);
            return "";
        }

        [RestMethod(Name = "/notify-ti", ParamCount = 1, ContentType = "application/json")]
        public string NotifyTripInstance(string[] args)
        {
            var tripInstanceId = long.Parse(args[0]);
            TripInstance tripInstance;
            using (var context = ApplicationDbContext.Create())
            {
                tripInstance = context.TripInstances.Include(ti => ti.UserTripInstances).FirstOrDefault(ti => ti.Id == tripInstanceId);
            }
            if (tripInstance == null)
                return "";
            // Only re-send notifications if the final notification has been sent.
            if (tripInstance.UserTripInstances.Any(uti => uti.FinalNotificationTime != null))
            {
                NotificationManager.GetInstance().SendFinalNotification(tripInstanceId, true);
            }
            return "";
        }
    }
}
