using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
                    var clientTrip = model.Trips.FirstOrDefault(t => t.Id == serverUserTrip.TripId);
                    if (clientTrip == null || !clientTrip.UserTrips.Contains(user.Id))
                        continue;
                    var clientUserTrip = clientTrip.UserTrips[user.Id];
                    if (!clientUserTrip.Attending)
                        continue;
                    foreach (var serverUserTripInstance in serverUserTrip.Instances)
                    {
                        var clientTripInstance = clientTrip.Instances.FirstOrDefault(tr => tr.Id == serverUserTripInstance.TripInstanceId);
                        if (clientTripInstance == null || !clientTripInstance.UserTripInstances.Contains(user.Id))
                            continue;
                        var clientUserTripInstance = clientTripInstance.UserTripInstances[user.Id];
                        if (clientUserTripInstance.Attending == true && (serverUserTripInstance.Attending != true
                            || serverUserTripInstance.CommuteMethod != clientUserTripInstance.CommuteMethod
                            || serverUserTripInstance.CanDriveIfNeeded != clientUserTripInstance.CanDriveIfNeeded
                            || serverUserTripInstance.Seats != clientUserTripInstance.Seats))
                        {
                            serverUserTripInstance.CommuteMethod = clientUserTripInstance.CommuteMethod;
                            serverUserTripInstance.CanDriveIfNeeded = clientUserTripInstance.CanDriveIfNeeded;
                            serverUserTripInstance.Seats = clientUserTripInstance.Seats;
                            if (serverUserTripInstance.Attending != true)
                            {
                                // User wants to attend. Make sure there is space.
                                if (serverUserTripInstance.ConfirmTime == null)
                                    serverUserTripInstance.ConfirmTime = DateTime.UtcNow;
                                serverUserTripInstance.Attending = true;
                                if (serverUserTripInstance.CommuteMethod == CommuteMethod.NeedRide && !serverUserTripInstance.CanDriveIfNeeded)
                                {
                                    if (serverUserTripInstance.TripInstance.DriversPicked)
                                    {
                                        // Drivers have already been picked. Make sure there is enough room for this user.
                                        var requiredSeats = serverUserTripInstance.TripInstance.GetRequiredSeats();
                                        var availableSeats = serverUserTripInstance.TripInstance.GetAvailableSeats();
                                        if (requiredSeats > availableSeats)
                                        {
                                            serverUserTripInstance.Attending = false;
                                            serverUserTripInstance.NoRoom = true;
                                            serverModel.SetMessage("You cannot attend because there are not enough seats. You have been added to the waiting list.", MessageType.Error);
                                        }
                                    }
                                }
                            }
                            save = true;
                        }
                        else if (clientUserTripInstance.Attending == false && serverUserTripInstance.Attending != false)
                        {
                            serverUserTripInstance.Attending = false;
                            serverUserTripInstance.ConfirmTime = null;
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

        [AllowAnonymous]
        public ActionResult Version()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return Content(version.ToString(), "text/plain");
        }

        private static HomeViewModel GetHomeViewModel(ApplicationDbContext context)
        {
            var model = new HomeViewModel();
            var user = AppUtils.CurrentUser;
            if (user != null && user.Status == UserStatus.Active)
            {
                model.Users = context.Users.Where(u => u.Status == UserStatus.Active).ToList();
                model.Trips = context.Trips
                    .Where(t => t.UserTrips.Any(ut => ut.UserId == user.Id && ut.Attending))
                    .Include(t => t.Recurrences)
                    .Include(t => t.UserTrips.Select(ut => ut.Instances))
                    .ToList();
                foreach (var tripRecurrence in model.Trips.SelectMany(t => t.Recurrences))
                {
                    // With EF magic, this method automatically adds the next TripInstance for each recurrence to the Trip
                    var instance = context.GetNextTripInstance(tripRecurrence, ApplicationDbContext.TripInstanceRemovalDelay);
                    if (instance != null)
                        context.GetTripInstanceById(instance.Id);
                }
                bool save = false;
                foreach (var trip in model.Trips)
                {
                    trip.Instances = trip.Instances.OrderBy(ti => ti.Date).ToList();
                    UserTrip userTrip;
                    if (trip.UserTrips.Contains(user.Id))
                        userTrip = trip.UserTrips[user.Id];
                    else
                        continue;
                    foreach (var tripInstance in trip.Instances)
                    {
                        var userTripInstance = userTrip.Instances.FirstOrDefault(uti => uti.TripInstanceId == tripInstance.Id);
                        if (userTripInstance == null)
                        {
                            // Create the UserTripInstance if it doesn't exist
                            userTripInstance = UserTripInstance.Create(user, tripInstance);
                            userTripInstance.Attending = false;
                            // Set User to null, otherwise EF might think we are trying to create a new user.
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
