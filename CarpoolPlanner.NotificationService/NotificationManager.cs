using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using CarpoolPlanner.Model;
using log4net;
using TextNow.Net;
using Timer = System.Timers.Timer;

namespace CarpoolPlanner.NotificationService
{
    // Note: this class WILL be accessed from multiple threads, so all methods must be thread-safe.
    public class NotificationManager
    {
        // TODO: make these settings not hard-coded
        public static readonly TimeSpan InitialAdvanceNotificationTime = TimeSpan.FromHours(4);
        public static readonly TimeSpan SecondAdvanceNotificationTime = TimeSpan.FromHours(1);
        public static readonly TimeSpan FinalAdvanceNotificationTime = TimeSpan.FromMinutes(30);

        private static readonly ILog log = LogManager.GetLogger(typeof(NotificationManager));
        private static readonly object instanceLock = new object();
        private static NotificationManager instance;

        private static ConcurrentDictionary<string, long?> lastMessageIds = new ConcurrentDictionary<string, long?>();

        public static NotificationManager GetInstance()
        {
            if (instance != null)
                return instance;

            lock (instanceLock)
            {
                return instance ?? (instance = new NotificationManager());
            }
        }

        private readonly Dictionary<long, Timer> initialTimers = new Dictionary<long, Timer>();
        private readonly Dictionary<long, Timer> secondTimers = new Dictionary<long, Timer>();
        private readonly Dictionary<long, Timer> finalTimers = new Dictionary<long, Timer>();
        private readonly Dictionary<long, Timer> nextInstanceTimers = new Dictionary<long, Timer>();
        private readonly Dictionary<long, Timer> receiveMessageTimers = new Dictionary<long, Timer>();
        private readonly Dictionary<long, TripInstance> prevTripInstances = new Dictionary<long, TripInstance>();
        private TextNowClient client;

        private NotificationManager()
        {
            client = new TextNowClient(ConfigurationManager.AppSettings["TextNowFromName"],
                ConfigurationManager.AppSettings["TextNowUsername"],
                ConfigurationManager.AppSettings["TextNowPassword"]);
        }

        public void Init()
        {
            const int maxRetryCount = 20;
            using (var context = ApplicationDbContext.Create())
            {
                int retryCount = 0;
                bool success = false;
                do
                {
                    try
                    {
                        foreach (var user in context.Users)
                        {
                            lastMessageIds.AddOrUpdate(user.Id, user.LastTextMessageId, (id, m) => m);
                        }
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        Program.HandleException(ex);
                        retryCount++;
                    }
                } while (!success && retryCount < maxRetryCount);
                do
                {
                    try
                    {
                        foreach (var tripRecurrence in context.TripRecurrences.ToList())
                        {
                            var tripInstance = context.GetNextTripInstance(tripRecurrence, ApplicationDbContext.TripInstanceRemovalDelay);
                            SetNextNotificationTimes(tripInstance, tripRecurrence.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.HandleException(ex);
                        retryCount++;
                    }
                } while (!success && retryCount < maxRetryCount);
            }
        }

        public void SetNextNotificationTimes(TripInstance tripInstance, long tripRecurrenceId)
        {
            if (tripInstance == null)
                return;
            // Ask people if they are coming
            SetNextNotificationTime(tripInstance.Date - InitialAdvanceNotificationTime, tripInstance.Id, initialTimers, SendInitialNotification);
            // Ask anyone who didn't respond again
            SetNextNotificationTime(tripInstance.Date - SecondAdvanceNotificationTime, tripInstance.Id, secondTimers, SendInitialNotification);
            // Tell attendees who is coming & driving
            SetNextNotificationTime(tripInstance.Date - FinalAdvanceNotificationTime, tripInstance.Id, finalTimers, SendFinalNotification);
            // Clean up
            SetNextNotificationTime(tripInstance.Date + ApplicationDbContext.TripInstanceRemovalDelay, tripRecurrenceId, nextInstanceTimers,
                (id) => LoadNextTripInstance(id, tripInstance.Id));
        }

        private void SetNextNotificationTime(DateTime time, long id, Dictionary<long, Timer> dictionary, Action<long> action)
        {
            if (time <= DateTime.Now)
            {
                try
                {
                    action(id);
                }
                catch (Exception ex)
                {
                    Program.HandleException(ex);
                }
                return;
            }
            lock (dictionary)
            {
                Timer timer;
                if (!dictionary.TryGetValue(id, out timer))
                {
                    timer = new Timer();
                    timer.Elapsed += (sender, e) =>
                    {
                        try
                        {
                            timer.Stop();
                            lock (dictionary)
                            {
                                dictionary.Remove(id);
                            }
                            action(id);
                        }
                        catch (Exception ex)
                        {
                            Program.HandleException(ex);
                        }
                    };
                    dictionary.Add(id, timer);
                }
                // Note: if changing the interval while the timer is running, the timer will restart with the new interval.
                timer.Interval = (time - DateTime.Now).TotalMilliseconds;
                timer.Start();
            }
        }

        /// <summary>
        /// Sends a notification message to the specified user.
        /// </summary>
        /// <param name="user">The user to whom the notification will be sent.</param>
        /// <param name="message">The message to send</param>
        /// <param name="force">If true, will send the message even if the user's notification settings are all turned off (assuming the user has specified an e-mail or phone number).</param>
        public async Task SendNotification(User user, string message, bool force = false)
        {
            bool sendEmail = user.EmailNotify && !string.IsNullOrEmpty(user.Email);
            bool sendSms = user.PhoneNotify && !string.IsNullOrEmpty(user.Phone);
            if (force && !sendEmail && !sendSms)
            {
                // When force-sending, prefer e-mail because it is less intrusive (especially if the user pays for incoming texts)
                if (!string.IsNullOrEmpty(user.Email))
                    sendEmail = true;
                else if (!string.IsNullOrEmpty(user.Phone))
                    sendSms = true;
            }
            if (sendEmail)
            {
                // TODO: send e-mail
            }
            if (sendSms)
            {
                long? messageId;
                int retryCount = 0;
                const int maxRetryCount = 20;
                do
                {
                    if (retryCount > 0)
                    {
                        log.WarnFormat("Failed to send message to {0}. Retrying in a little while ({1})", retryCount);
                        await Task.Delay(10);
                    }
                    messageId = await client.SendMessage(message, user.Phone).ConfigureAwait(false);
                    retryCount++;
                } while (messageId == null && retryCount < maxRetryCount);
                if (messageId != null)
                {
                    user.LastTextMessageId = messageId;
                    lastMessageIds.AddOrUpdate(user.Id, messageId, (id, m) => messageId);
                }
                else
                {
                    log.ErrorFormat("Failed to send message to {0}. Exceeded maximum retry count.", user.Phone);
                }
            }
        }

        /// <summary>
        /// Sends a notification message to the specified user.
        /// </summary>
        /// <param name="user">The user to whom the notification will be sent.</param>
        /// <param name="message">The message to send</param>
        /// <param name="force">If true, will send the message even if the user's notification settings are all turned off (assuming the user has specified an e-mail or phone number).</param>
        public async Task SendSMS(User user, string message)
        {
            var messageId = await client.SendMessage(message, user.Phone).ConfigureAwait(false);
            if (messageId != null)
            {
                user.LastTextMessageId = messageId;
            }
        }

        /// <summary>
        /// Receives and handles SMS messages from the specified user.
        /// </summary>
        /// <param name="user">A user.</param>
        public async void ReceiveMessages(long tripInstanceId)
        {
            bool statusChanged = false;
            using (var context = ApplicationDbContext.Create())
            {
                var tripInstance = context.GetTripInstanceById(tripInstanceId);
                var tasks = new List<Task>(tripInstance.UserTripInstances.Count);
                foreach (var userTripInstance in tripInstance.UserTripInstances)
                {
                    if (string.IsNullOrEmpty(userTripInstance.User.Phone))
                        continue;
                    tasks.Add(Task.Run(async () =>
                    {
                        bool changed = false;
                        var lastMessageId = lastMessageIds.GetOrAdd(userTripInstance.UserId, (long?)null);
                        var messages = await client.ReceiveMessages(userTripInstance.User.Phone, lastMessageId).ConfigureAwait(false);
                        if (messages == null)
                        {
                            log.WarnFormat("Failed to receive messages (user {0}; trip instance {1})", userTripInstance.UserId, tripInstanceId);
                            return;
                        }
                        foreach (var message in messages)
                        {
                            userTripInstance.User.LastTextMessageId = message.Id;
                            lastMessageIds.AddOrUpdate(userTripInstance.UserId, message.Id, (id, m) => message.Id);
                            changed = true;
                            if (message.Date <= DateTime.Now - TimeSpan.FromHours(5.5))// TODO: use the actual 1st notification time, or slightly before it
                            {
                                // Message was sent before the 1st notification. Ignore it.
                                log.WarnFormat("Ignoring message '{0}' with ID {1} from user {2} because it was sent too long ago ({3})",
                                    message.Value, message.Id, userTripInstance.UserId, message.Date.ToString("dd-MMM-yyyy hh:mm:ss"));
                                continue;
                            }
                            string messageValue = message.Value.ToLower();
                            // TODO: if the user schedules multiple trip instances at the same time, it is ambiguous which trip they are referring to.
                            // This is fine for now because there is only one trip, but in the future add a way of disambiguating.
                            bool understood = false;
                            if (Regex.IsMatch(messageValue, @"\byes\b"))
                            {
                                understood = true;
                                if (userTripInstance.Attending != true)
                                {
                                    if (userTripInstance.ConfirmTime == null)
                                    {
                                        // TODO: account for time zones
                                        // To prevent people from cheating by sending false message times, limit the message time to be at most 2 minutes ago.
                                        if (message.Date == default(DateTime))
                                            message.Date = DateTime.Now;
                                        var minDate = DateTime.Now - TimeSpan.FromMinutes(2);
                                        if (message.Date < minDate)
                                            message.Date = minDate;
                                        userTripInstance.ConfirmTime = message.Date;
                                    }
                                    userTripInstance.Attending = true;
                                    if (tripInstance.DriversPicked)
                                    {
                                        // Drivers have already been picked. Make sure there is enough room for this user.
                                        var requiredSeats = tripInstance.GetRequiredSeats();
                                        var availableSeats = tripInstance.GetAvailableSeats();
                                        if (requiredSeats > availableSeats)
                                        {
                                            userTripInstance.Attending = false;
                                            userTripInstance.NoRoom = true;
                                            SendNotification(userTripInstance.User, "You cannot attend because there are not enough seats. You have been added to the waiting list.");
                                        }
                                    }
                                    if (userTripInstance.Attending == true)
                                    {
                                        statusChanged = true;
                                    }
                                }
                            }
                            else if (Regex.IsMatch(messageValue, @"\bno\b") || messageValue.Contains("not coming"))
                            {
                                understood = true;
                                if (userTripInstance.Attending != false)
                                {
                                    userTripInstance.Attending = false;
                                    userTripInstance.ConfirmTime = null;
                                    statusChanged = true;
                                }
                            }
                            if (messageValue.Contains("not driving"))
                            {
                                understood = true;
                                if (userTripInstance.CommuteMethod == CommuteMethod.Driver)
                                {
                                    if (userTripInstance.User.CommuteMethod == CommuteMethod.HaveRide)
                                        userTripInstance.CommuteMethod = CommuteMethod.HaveRide;
                                    else
                                        userTripInstance.CommuteMethod = CommuteMethod.NeedRide;
                                    userTripInstance.CanDriveIfNeeded = false;
                                    statusChanged = true;
                                }
                            }
                            if (!understood)
                            {
                                // TODO: don't hard-code the url
                                SendSMS(userTripInstance.User, "Sorry, I didn't understand that.\nFor more details, see http://climbing.pororeplays.com/Notifications.aspx");
                            }
                        }
                        if (changed)
                            context.SaveChanges();
                    }));
                }
                lock (prevTripInstances)
                {
                    if (DateTime.Now > tripInstance.Date - FinalAdvanceNotificationTime)
                    {
                        if (statusChanged)
                        {
                            // Send a final notification update
                            // TODO: send a special message that highlights the change, instead of just sending the whole thing again.
                            // But make sure to mention if there are not enough drivers.
                            SendFinalNotification(tripInstanceId, true);
                        }
                        else
                        {
                            // TEMP CODE for running NotificationService on my PC.
                            // Check the DB to see if any data changed from the web app.
                            if (tripInstance.DriversPicked)
                            {
                                TripInstance prevValue;
                                if (prevTripInstances.TryGetValue(tripInstanceId, out prevValue) && HasCommuteDataChanged(tripInstance, prevValue))
                                {
                                    SendFinalNotification(tripInstanceId, true);
                                }
                            }
                            // end temp code
                        }
                    }
                    prevTripInstances[tripInstanceId] = tripInstance;
                }
                // Wait for tasks to finish before disposing the DB context.
                await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
            }
        }

        private static bool HasCommuteDataChanged(TripInstance a, TripInstance b)
        {
            if (a.UserTripInstances.Count != b.UserTripInstances.Count)
                return true;
            for (int i = 0; i < a.UserTripInstances.Count; i++)
            {
                var utia = a.UserTripInstances[i];
                var utib = b.UserTripInstances[i];
                if (utia.Attending != utib.Attending || utia.CommuteMethod != utib.CommuteMethod || utia.NoRoom != utib.NoRoom || utia.Seats != utib.Seats)
                    return true;
            }
            return false;
        }

        private void SendInitialNotification(long tripInstanceId)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var tripInstance = context.GetTripInstanceById(tripInstanceId);
                context.SaveChanges();
                var sb = new StringBuilder(250);
                foreach (var userTripInstance in tripInstance.UserTripInstances.Where(uti => uti.Attending == null && uti.User.Status == UserStatus.Active))
                {
                    sb.Append("Are you coming to ");
                    sb.Append(tripInstance.Trip.Name);
                    sb.Append(" at ");
                    sb.Append(tripInstance.Date.ToString("h:mm tt"));
                    sb.Append("?\n");
                    bool isDriver = userTripInstance.CommuteMethod == CommuteMethod.Driver;
                    sb.Append("Reply with yes/no. \n");
                    sb.Append("For more options, go to http://climbing.pororeplays.com"); // TODO: don't hard-code the url
                    SendNotification(userTripInstance.User, sb.ToString());
                    sb.Clear();
                }
                // Now periodically attempt to receive messages to get responses.
                lock (receiveMessageTimers)
                {
                    Timer timer;
                    if (!receiveMessageTimers.TryGetValue(tripInstanceId, out timer))
                    {
                        timer = new Timer();
                        timer.Elapsed += (sender, e) => ReceiveMessages(tripInstanceId);
                        receiveMessageTimers.Add(tripInstanceId, timer);
                    }
                    timer.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
                    timer.Start();
                }
                context.SaveChanges();
            }
        }

        private void SendFinalNotification(long tripInstanceId)
        {
            SendFinalNotification(tripInstanceId, false);
        }

        private void SendFinalNotification(long tripInstanceId, bool isUpdate)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var tripInstance = context.GetTripInstanceById(tripInstanceId);
                PickDrivers(tripInstance);
                var availableSeats = tripInstance.GetAvailableSeats();
                var requiredSeats = tripInstance.GetRequiredSeats();
                context.SaveChanges();
                var utisToNotify = from uti in tripInstance.UserTripInstances
                                   where (uti.Attending == true || uti.NoRoom)
                                     && uti.User.Status == UserStatus.Active
                                   select uti;
                var sb = new StringBuilder(125);
                foreach (var userTripInstance in utisToNotify)
                {
                    if (isUpdate)
                        sb.Append("UPDATE: ");

                    if (userTripInstance.CommuteMethod == CommuteMethod.Driver)
                    {
                        sb.Append("You are driving for ");
                    }
                    else if (userTripInstance.Attending == true)
                    {
                        sb.Append("You are attending ");
                        if (userTripInstance.User.CommuteMethod == CommuteMethod.Driver)
                            sb.Append("(but not driving for) ");
                    }
                    else if (userTripInstance.NoRoom)
                    {
                        sb.Append("There are NOT enough seats for you to attend ");
                    }
                    sb.Append(tripInstance.Trip.Name);
                    sb.Append(" at ");
                    sb.Append(tripInstance.Date.ToString("h:mm tt"));
                    sb.AppendFormat(".\nThere are {0} seats and {1} people coming.\n", availableSeats, requiredSeats);
                    sb.Append("\n" + tripInstance.GetStatusReport() + "\n");
                    sb.Append("\nFor more details, go to http://climbing.pororeplays.com"); // TODO: don't hard-code the url
                    SendNotification(userTripInstance.User, sb.ToString());
                    sb.Clear();
                }
                context.SaveChanges();
                if (!isUpdate)
                {
                    lock (prevTripInstances)
                    {
                        prevTripInstances[tripInstanceId] = tripInstance;
                    }
                }
            }
        }

        private void LoadNextTripInstance(long tripRecurrenceId, long oldTripInstanceId)
        {
            using (var context = ApplicationDbContext.Create())
            {
                lock (receiveMessageTimers)
                {
                    Timer timer;
                    if (receiveMessageTimers.TryGetValue(oldTripInstanceId, out timer))
                    {
                        timer.Stop();
                        receiveMessageTimers.Remove(oldTripInstanceId);
                    }
                }
                var tripRecurrence = context.TripRecurrences.Find(tripRecurrenceId);
                var tripInstance = context.GetNextTripInstance(tripRecurrence, TimeSpan.Zero);
                SetNextNotificationTimes(tripInstance, tripRecurrence.Id);
            }
        }

        public void PickDrivers(TripInstance tripInstance)
        {
            try
            {
                // If any users were kicked before, re-add them to try to include them now. They will be re-kicked if there are still not enough seats.
                foreach (var userTripInstance in tripInstance.UserTripInstances.Where(uti => uti.NoRoom))
                {
                    userTripInstance.Attending = true;
                    userTripInstance.NoRoom = false;
                }
                var availableSeats = tripInstance.GetAvailableSeats();
                var requiredSeats = tripInstance.GetRequiredSeats();
                var difference = availableSeats - requiredSeats;
                var rnd = new Random();
                if (difference < 0)
                {
                    // Need more drivers
                    var availableDriverGroups = tripInstance.UserTripInstances
                        .Where(uti => uti.CanDriveIfNeeded && uti.CommuteMethod != CommuteMethod.Driver && (uti.Attending == true || uti.NoRoom) && uti.User.Status == UserStatus.Active)
                        .GroupBy(uti => uti.UserTrip.GetDrivingRatio())
                        .OrderBy(g => g.Key);
                    // Start with drivers with the lowest driving ratio and add drivers until there are enough.
                    foreach (var group in availableDriverGroups)
                    {
                        // Add drivers in order of most seats to least seats
                        var subGroups = group.GroupBy(uti => uti.Seats).OrderByDescending(g => g.Key);
                        foreach (var subGroup in subGroups)
                        {
                            var availableDrivers = subGroup.ToList();
                            int addDriverCount = (int)Math.Ceiling((double)(-difference) / (double)subGroup.Key);
                            List<UserTripInstance> addedDrivers;
                            if (addDriverCount >= availableDrivers.Count)
                            {
                                addedDrivers = availableDrivers;
                            }
                            else
                            {
                                addedDrivers = new List<UserTripInstance>(addDriverCount);
                                var index = rnd.Next(availableDrivers.Count);
                                addedDrivers.Add(availableDrivers[index]);
                                availableDrivers.RemoveAt(index);
                            }
                            foreach (UserTripInstance driver in addedDrivers)
                                driver.CommuteMethod = CommuteMethod.Driver;
                            availableSeats += addedDrivers.Count * subGroup.Key;
                            difference = availableSeats - requiredSeats;
                            if (difference >= 0)
                                break;
                        }
                        if (difference >= 0)
                            break;
                    }
                    if (difference < 0)
                    {
                        // Still not enough seats. Start kicking out the non-drivers who confirmed late.
                        var usersToKick = (from uti in tripInstance.UserTripInstances
                                           where uti.Attending == true && uti.CommuteMethod == CommuteMethod.NeedRide && uti.User.Status == UserStatus.Active
                                           orderby uti.ConfirmTime descending
                                           select uti)
                                          .Take(-difference);
                        foreach (var uti in usersToKick)
                        {
                            uti.Attending = false;
                            uti.NoRoom = true;
                            requiredSeats--;
                        }
                        difference = availableSeats - requiredSeats;
                    }
                }
                // Note: DON'T use "else if" here because the previous block could have added too many drivers (in the case where drivers have different number of seats).
                // Add a buffer when choosing drivers in case more people show up last minute.
                // TODO: make this a setting
                const int seatsBuffer = 1;
                while (difference > seatsBuffer)
                {
                    // More than enough drivers. See if we can remove some.
                    // Start with drivers with the highest driving ratio and remove as many drivers as possible.
                    var group = tripInstance.UserTripInstances
                        .Where(uti => uti.CommuteMethod == CommuteMethod.Driver && uti.Seats <= difference - seatsBuffer && uti.User.Status == UserStatus.Active && uti.Attending == true)
                        .GroupBy(uti => uti.UserTrip.GetDrivingRatio())
                        .OrderByDescending(g => g.Key)
                        .FirstOrDefault();
                    if (group == null)
                        break;
                    // Remove drivers in order of least seats to most seats
                    var subGroup = group.GroupBy(uti => uti.Seats).OrderBy(g => g.Key).First();
                    var availableDrivers = subGroup.ToList();
                    int removeDriverCount = (difference - seatsBuffer) / subGroup.Key;
                    List<UserTripInstance> removedDrivers;
                    if (removeDriverCount >= availableDrivers.Count)
                    {
                        removedDrivers = availableDrivers;
                    }
                    else
                    {
                        removedDrivers = new List<UserTripInstance>(removeDriverCount);
                        var index = rnd.Next(availableDrivers.Count);
                        removedDrivers.Add(availableDrivers[index]);
                        availableDrivers.RemoveAt(index);
                    }
                    foreach (UserTripInstance driver in removedDrivers)
                        driver.CommuteMethod = CommuteMethod.NeedRide;
                    availableSeats -= removedDrivers.Count * subGroup.Key;
                    difference = availableSeats - requiredSeats;
                };
                tripInstance.DriversPicked = true;
            }
            catch (Exception ex)
            {
                log.Error("Error picking drivers: " + ex.ToString());
            }
        }
    }
}