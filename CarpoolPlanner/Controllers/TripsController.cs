using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarpoolPlanner.Model;
using CarpoolPlanner.ViewModel;
using log4net;

namespace CarpoolPlanner.Controllers
{
    [Authorize]
    public class TripsController : CarpoolControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TripsController));

        public ActionResult Index()
        {
            var model = new TripsCombinedViewModel();
            using (var context = ApplicationDbContext.Create())
            {
                model.TripsModel = GetTripsViewModel(context);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(TripsViewModel model)
        {
            // We should only be saving the attendance here,
            // so load the model form the DB and update the attendance values.
            // DON'T simly save the clientModel to the DB.
            if (model == null)
            {
                Response.StatusCode = 400;
                model = new TripsViewModel();
                model.SetMessage("Model not specified.", MessageType.Error);
                return Ng(model);
            }
            if (model.Trips == null)
            {
                model.SetMessage("No changes to save.", MessageType.Error);
                return Ng(model);
            }
            TripsViewModel serverModel;
            var user = AppUtils.CurrentUser;
            bool save = false;
            using (var context = ApplicationDbContext.Create())
            {
                serverModel = GetTripsViewModel(context);
                foreach (var serverUserTrip in from t in serverModel.Trips
                                         where t.UserTrips.Contains(user.Id)
                                         select t.UserTrips[AppUtils.CurrentUser.Id])
                {
                    var clientUserTrip = model.Trips.Where(t => t.Id == serverUserTrip.TripId && t.UserTrips.Contains(AppUtils.CurrentUser.Id))
                        .Select(t => t.UserTrips[AppUtils.CurrentUser.Id])
                        .FirstOrDefault();
                    if (clientUserTrip == null)
                        continue;
                    if (clientUserTrip.Attending != serverUserTrip.Attending)
                    {
                        serverUserTrip.Attending = clientUserTrip.Attending;
                        save = true;
                    }
                    if (serverUserTrip.Attending)
                    {
                        foreach (var recurrence in serverUserTrip.Recurrences)
                        {
                            var clientRecurrence = clientUserTrip.Recurrences.FirstOrDefault(utr => utr.TripRecurrenceId == recurrence.TripRecurrenceId);
                            if (clientRecurrence == null)
                                continue;
                            if (clientRecurrence.Attending != recurrence.Attending)
                            {
                                recurrence.Attending = clientRecurrence.Attending;
                                if (recurrence.Attending)
                                {
                                    // Ensure the trip instance exists
                                    context.GetNextUserTripInstance(recurrence.TripRecurrence, AppUtils.CurrentUser);
                                    // TODO: send message to notification service
                                }
                                save = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var recurrence in serverUserTrip.Recurrences.Where(utr => utr.Attending))
                        {
                            recurrence.Attending = false;
                            save = true;
                        }
                    }
                }
                if (save)
                    context.SaveChanges();
            }
            serverModel.SetMessage(save ? "Saved successfully." : "No changes to save.", MessageType.Success);
            return Ng(serverModel);
        }

        [HttpPost]
        public ActionResult CreateTrip(CreateTripViewModel model)
        {
            if (!AppUtils.IsUserAdmin())
            {
                Response.StatusCode = 403;
                model.SetMessage("You are not authorized to create trips.", MessageType.Error);
                log.Warn(model.Message);
                return Ng(model);
            }
            using (var context = ApplicationDbContext.Create())
            {
                context.Trips.Add(model.Trip);
                context.SaveChanges();
                model.CreatedTrip = model.Trip;
                if (EnsureUserTrips(context, model.CreatedTrip, AppUtils.CurrentUser))
                    context.SaveChanges();
            }
            model.Trip = new Trip();
            model.Trip.Recurrences.Add(new TripRecurrence());
            model.SetMessage("Created successfully.", MessageType.Success);
            return Ng(model);
        }

        [HttpDelete]
        public ActionResult DeleteTrip(int id)
        {
            var model = new TripsViewModel();
            if (!AppUtils.IsUserAdmin())
            {
                Response.StatusCode = 403;
                model.SetMessage("You are not authorized to delete trips.", MessageType.Error);
                log.Warn(model.Message);
                return Ng(model);
            }
            using (var context = ApplicationDbContext.Create())
            {
                var trip = context.Trips.Find(id);
                if (trip != null)
                {
                    context.Trips.Remove(trip);
                    context.SaveChanges();
                    model.SetMessage("Deleted successfully.", MessageType.Success);
                }
                else
                {
                    model.SetMessage("Trip does not exist. It may have been deleted by another user..", MessageType.Error);
                }
            }
            return Ng(model);
        }

        private static TripsViewModel GetTripsViewModel(ApplicationDbContext context)
        {
            var model = new TripsViewModel();
            var user = AppUtils.CurrentUser;
            model.Trips = context.Trips.Include(t => t.Recurrences).ToList();
            context.GetUserTrips(user.Id).Include(ut => ut.Recurrences).ToList();

            bool save = false;
            // Create UserTrips and UserTripRecurrences if they don't exist for this user.
            foreach (var trip in model.Trips)
            {
                save = EnsureUserTrips(context, trip, user) || save;
            }
            if (save)
                context.SaveChanges();
            return model;
        }

        private static bool EnsureUserTrips(ApplicationDbContext context, Trip trip, User user)
        {
            bool save = false;
            UserTrip userTrip;
            if (trip.UserTrips.Contains(user.Id))
            {
                userTrip = trip.UserTrips[user.Id];
            }
            else
            {
                userTrip = new UserTrip { Attending = false, UserId = AppUtils.CurrentUser.Id, TripId = trip.Id };
                trip.UserTrips.Add(userTrip);
                context.UserTrips.Add(userTrip);
                save = true;
            }
            foreach (var tripRecurrence in trip.Recurrences)
            {
                if (userTrip.Recurrences.All(utr => utr.TripRecurrenceId != tripRecurrence.Id))
                {
                    var userTripRecurrence = new UserTripRecurrence
                    {
                        Attending = false,
                        TripId = trip.Id,
                        TripRecurrenceId = tripRecurrence.Id,
                        UserId = AppUtils.CurrentUser.Id
                    };
                    userTrip.Recurrences.Add(userTripRecurrence);
                    context.UserTripRecurrences.Add(userTripRecurrence);
                    save = true;
                }
            }
            return save;
        }
    }
}
