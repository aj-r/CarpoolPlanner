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
            TripsViewModel model;
            using (var context = ApplicationDbContext.Create())
            {
                model = GetTripsViewModel(context);
            }
            model.Create.Trip = new Trip();
            model.Create.Trip.Recurrences.Add(new TripRecurrence());
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(TripsViewModel model)
        {
            // We should only be saving the attendance here,
            // so load the model form the DB and update the attendance values.
            // DON'T simly save the clientModel to the DB.
            TripsViewModel serverModel;
            bool save = false;
            using (var context = ApplicationDbContext.Create())
            {
                serverModel = GetTripsViewModel(context);
                foreach (var userTrip in serverModel.Trips.Select(t => t.UserTrips[AppUtils.CurrentUser.Id]))
                {
                    var clientUserTrip = model.Trips.Where(t => t.Id == userTrip.TripId && t.UserTrips.Contains(AppUtils.CurrentUser.Id))
                        .Select(t => t.UserTrips[AppUtils.CurrentUser.Id])
                        .FirstOrDefault();
                    if (clientUserTrip == null)
                        continue;
                    if (clientUserTrip.Attending != userTrip.Attending)
                    {
                        userTrip.Attending = clientUserTrip.Attending;
                        save = true;
                    }
                    if (userTrip.Attending)
                    {
                        foreach (var recurrence in userTrip.Recurrences)
                        {
                            var clientRecurrence = clientUserTrip.Recurrences.FirstOrDefault(utr => utr.TripRecurrenceId == recurrence.TripRecurrenceId);
                            if (clientRecurrence == null)
                                continue;
                            if (clientRecurrence.Attending != recurrence.Attending)
                            {
                                recurrence.Attending = clientRecurrence.Attending;
                                save = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var recurrence in userTrip.Recurrences.Where(utr => utr.Attending))
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
            // TODO: validate trip
            using (var context = ApplicationDbContext.Create())
            {
                context.Trips.Add(model.Trip);
                context.SaveChanges();
                model.CreatedTrip = model.Trip;
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

        private TripsViewModel GetTripsViewModel(ApplicationDbContext context)
        {
            var model = new TripsViewModel();
            model.Trips = context.Trips.Include(t => t.Recurrences).ToList();
            context.GetUserTrips(AppUtils.CurrentUser.Id).Include(ut => ut.Recurrences).ToList();

            // Create UserTrips and UserTripRecurrences if they don't exist for this user.
            foreach (var trip in model.Trips)
            {
                if (!trip.UserTrips.Contains(AppUtils.CurrentUser.Id))
                {
                    var userTrip = new UserTrip { Attending = false, UserId = AppUtils.CurrentUser.Id, TripId = trip.Id };
                    foreach (var tripRecurrence in trip.Recurrences)
                    {
                        var userTripRecurrence = new UserTripRecurrence
                        {
                            Attending = false,
                            TripId = trip.Id,
                            TripRecurrenceId = tripRecurrence.Id,
                            UserId = AppUtils.CurrentUser.Id
                        };
                        userTrip.Recurrences.Add(userTripRecurrence);
                    }
                    trip.UserTrips.Add(userTrip);
                }
            }
            return model;
        }
    }
}
