using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarpoolPlanner.Model;
using CarpoolPlanner.ViewModel;

namespace CarpoolPlanner.Controllers
{
    [Authorize]
    public class HomeController : CarpoolControllerBase
    {
        public ActionResult Index()
        {
            using (var context = ApplicationDbContext.Create())
            {
                var model = GetHomeViewModel(context);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Index(HomeViewModel model)
        {
            if (model == null)
            {
                Response.StatusCode = 400;
                model = new HomeViewModel();
                model.SetMessage("Model not specified.", MessageType.Error);
                return Ng(model);
            }
            if (model.Trips == null)
            {
                model.SetMessage("No changes to save.", MessageType.Error);
                return Ng(model);
            }
            var user = AppUtils.CurrentUser;
            HomeViewModel serverModel;
            bool save = false;
            using (var context = ApplicationDbContext.Create())
            {
                serverModel = GetHomeViewModel(context);
                foreach (var serverUserTrip in from t in serverModel.Trips
                                         where t.UserTrips.Contains(user.Id)
                                               select t.UserTrips[user.Id])
                {
                    var clientUserTrip = model.Trips.Where(t => t.Id == serverUserTrip.TripId && t.UserTrips.Contains(user.Id))
                        .Select(t => t.UserTrips[user.Id])
                        .FirstOrDefault();
                    if (clientUserTrip == null)
                        continue;
                    foreach (var serverInstance in serverUserTrip.Instances)
                    {
                        var clientInstance = clientUserTrip.Instances.FirstOrDefault(utr => utr.TripInstanceId == serverInstance.TripInstanceId);
                        if (clientInstance == null)
                            continue;
                        if (clientInstance.Attending == true && (serverInstance.Attending != true
                            ||serverInstance.CommuteMethod != clientInstance.CommuteMethod
                            || serverInstance.CanDriveIfNeeded != clientInstance.CanDriveIfNeeded
                            || serverInstance.Seats != clientInstance.Seats))
                        {
                            serverInstance.CommuteMethod = clientInstance.CommuteMethod;
                            serverInstance.CanDriveIfNeeded = clientInstance.CanDriveIfNeeded;
                            serverInstance.Seats = clientInstance.Seats;
                            if (serverInstance.Attending != true)
                            {
                                // User wants to attend. Make sure there is space.
                                if (serverInstance.ConfirmTime == null)
                                    serverInstance.ConfirmTime = DateTime.Now;
                                serverInstance.Attending = true;
                                if (serverInstance.CommuteMethod == CommuteMethod.NeedRide && !serverInstance.CanDriveIfNeeded)
                                {
                                    if (serverInstance.TripInstance.DriversPicked)
                                    {
                                        // Drivers have already been picked. Make sure there is enough room for this user.
                                        var requiredSeats = serverInstance.TripInstance.GetRequiredSeats();
                                        var availableSeats = serverInstance.TripInstance.GetAvailableSeats();
                                        if (requiredSeats > availableSeats)
                                        {
                                            serverInstance.Attending = false;
                                            serverInstance.NoRoom = true;
                                            serverModel.SetMessage("You cannot attend because there are not enough seats. You have been added to the waiting list.", MessageType.Error);
                                        }
                                    }
                                }
                            }
                            save = true;
                        }
                        else if (clientInstance.Attending == false && serverInstance.Attending != false)
                        {
                            serverInstance.Attending = false;
                            serverInstance.ConfirmTime = null;
                            save = true;
                        }
                    }
                }
                if (save)
                    context.SaveChanges();
                if (serverModel.MessageType != MessageType.Error)
                    serverModel.SetMessage(save ? "Saved successfully." : "No changes to save.", MessageType.Success);
            }
            return Ng(serverModel);
        }

        private static HomeViewModel GetHomeViewModel(ApplicationDbContext context)
        {
            var model = new HomeViewModel();
            if (AppUtils.IsUserStatus(UserStatus.Active))
            {
                model.Users = context.Users.Where(u => u.Status == UserStatus.Active).ToList();
                model.Trips = context.Trips
                    .Where(t => t.UserTrips.Any(ut => ut.UserId == AppUtils.CurrentUser.Id && ut.Attending))
                    .Include(t => t.Recurrences)
                    .Include(t => t.UserTrips.Select(ut => ut.Instances))
                    .ToList();
                foreach (var tripRecurrence in model.Trips.SelectMany(t => t.Recurrences))
                {
                    // With EF magic, this method automatically adds the next TripInstance for each recurrence to the Trip
                    var instance = context.GetNextTripInstance(tripRecurrence, ApplicationDbContext.TripInstanceRemovalDelay);
                    context.GetTripInstanceById(instance.Id);
                }
                bool save = false;
                foreach (var trip in model.Trips)
                {
                    trip.Instances = trip.Instances.OrderBy(ti => ti.Date).ToList();
                    UserTrip userTrip;
                    if (trip.UserTrips.Contains(AppUtils.CurrentUser.Id))
                        userTrip = trip.UserTrips[AppUtils.CurrentUser.Id];
                    else
                        continue;
                    foreach (var tripInstance in trip.Instances)
                    {
                        var userTripInstance = userTrip.Instances.FirstOrDefault(uti => uti.TripInstanceId == tripInstance.Id);
                        if (userTripInstance == null)
                        {
                            // Create the UserTripInstance if it doesn't exist
                            userTripInstance = UserTripInstance.Create(AppUtils.CurrentUser, tripInstance);
                            userTripInstance.User = null;
                            userTrip.Instances.Add(userTripInstance);
                            context.UserTripInstances.Add(userTripInstance);
                            save = true;
                        }
                    }
                }
                if (save)
                    context.SaveChanges();
            }
            return model;
        }
    }
}
