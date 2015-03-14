using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CarpoolPlanner.Model;
using log4net;

namespace CarpoolPlanner
{
    /// <summary>
    /// A client used for communication with the notification service.
    /// </summary>
    public class NotificationServiceClient
    {
        private const string baseUrl = "http://localhost:23122";
        private static readonly ILog log = LogManager.GetLogger(typeof(NotificationServiceClient));

        /// <summary>
        /// Updates the notification times for the specified trip instance. If notifications have already been sent, then any users that were not
        /// notified before will be notified now.
        /// </summary>
        /// <param name="tripInstanceId"></param>
        public void UpdateTripInstance(long tripInstanceId)
        {
            UpdateTripInstance(tripInstanceId, -1);
        }

        /// <summary>
        /// Updates the notification times for the specified trip instance. If notifications have already been sent, then any users that were not
        /// notified before will be notified now.
        /// </summary>
        /// <param name="tripInstanceId"></param>
        /// <param name="tripRecurrenceId"></param>
        public void UpdateTripInstance(long tripInstanceId, long tripRecurrenceId)
        {
            try
            {
                var uri = string.Concat(baseUrl, "/update-ti/", tripInstanceId, "/", tripRecurrenceId);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                SendRequestAsync(request);
            }
            catch (Exception ex)
            {
                log.Warn("Failed to send update message to notification service: " + ex.ToString());
            }
        }

        /// <summary>
        /// If the final notification has already been sent, then an update notification will be sent to all attendees.
        /// </summary>
        /// <param name="tripInstanceId"></param>
        public void NotifyTripInstance(long tripInstanceId)
        {
            try
            {
                var uri = string.Concat(baseUrl, "/notify-ti/", tripInstanceId);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                SendRequestAsync(request);
            }
            catch (Exception ex)
            {
                log.Warn("Failed to send update message to notification service: " + ex.ToString());
            }
        }

        /// <summary>
        /// Sends a notification to all admins informing them that a new user has registered on the website.
        /// </summary>
        /// <param name="userId">The ID of the newly registered user.</param>
        public void UserRegister(long userId)
        {
            try
            {
                var uri = string.Concat(baseUrl, "/user-register/", userId);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                SendRequestAsync(request);
            }
            catch (Exception ex)
            {
                log.Warn("Failed to send update message to notification service: " + ex.ToString());
            }
        }

        // NOTE: we don't use the async/await keywords here (although they would be nice) because they aren't supported on mono yet.
        private void SendRequestAsync(HttpWebRequest request)
        {
            var thread = new Thread(SendRequest);
            thread.Start(new Tuple<HttpWebRequest, User>(request, AppUtils.CurrentUser));
        }

        private void SendRequest(object args)
        {
            var tuple = args as Tuple<HttpWebRequest, User>;
            if (tuple == null)
            {
                log.Error("Failed to send request to notification service: tuple was null.");
                return;
            }
            try
            {
                var request = tuple.Item1;
                request.Accept = "*/*";
                request.GetResponse();
            }
            catch (Exception ex)
            {
                var user = tuple.Item2;
                if (user != null)
                    ThreadContext.Properties["UserId"] = user.Id;
                log.Warn("Failed to send request to notification service: " + ex.ToString());
            }
        }
    }
}