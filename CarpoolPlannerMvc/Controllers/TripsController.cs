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
    public class TripsController : CarpoolControllerBase
    {
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
        public ActionResult Index(TripsViewModel clientModel)
        {
            // We should only be saving the attendance here,
            // so load the model form the DB and update the attendance values.
            // DON'T simly save the clientModel to the DB.
            TripsViewModel model;
            bool save = false;
            using (var context = ApplicationDbContext.Create())
            {
                model = GetTripsViewModel(context);
                foreach (var userTrip in model.Trips.Select(t => t.UserTrips[AppUtils.CurrentUserId]))
                {
                    var clientUserTrip = clientModel.Trips.Where(t => t.Id == userTrip.TripId)
                        .Select(t => t.UserTrips[AppUtils.CurrentUserId])
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
            model.SetMessage(save ? "Saved successfully." : "No changes to save.", MessageType.Success);
            return Ng(model);
        }

        [HttpPost]
        public ActionResult CreateTrip(CreateTripViewModel createModel)
        {
            // TODO: validate trip
            using (var context = ApplicationDbContext.Create())
            {
                context.Trips.Add(createModel.Trip);
                context.SaveChanges();
                createModel.CreatedTrip = createModel.Trip;
            }
            createModel.Trip = new Trip();
            createModel.Trip.Recurrences.Add(new TripRecurrence());
            createModel.SetMessage("Created successfully.", MessageType.Success);
            return Ng(createModel);
        }

        [HttpDelete]
        public ActionResult DeleteTrip(int id)
        {
            var model = new TripsViewModel();
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
            context.GetUserTrips(AppUtils.CurrentUser).Include(ut => ut.Recurrences).ToList();

            // Create UserTrips and UserTripRecurrences if they don't exist for this user.
            foreach (var trip in model.Trips)
            {
                if (!trip.UserTrips.Contains(AppUtils.CurrentUserId))
                {
                    var userTrip = new UserTrip { Attending = false, UserId = AppUtils.CurrentUserId, TripId = trip.Id };
                    foreach (var tripRecurrence in trip.Recurrences)
                    {
                        var userTripRecurrence = new UserTripRecurrence
                        {
                            Attending = false,
                            TripId = trip.Id,
                            TripRecurrenceId = tripRecurrence.Id,
                            UserId = AppUtils.CurrentUserId
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
