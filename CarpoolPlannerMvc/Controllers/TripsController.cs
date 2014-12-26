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
            var model = new TripsViewModel();
            using (var context = ApplicationDbContext.Create())
            {
                model.Trips = context.Trips.Include(t => t.Recurrences).ToList();
                context.GetUserTrips(AppUtils.CurrentUser).Include(ut => ut.Recurrences).ToList();
            }
            model.Create.Trip = new Trip();
            model.Create.Trip.Recurrences.Add(new TripRecurrence());
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(TripsViewModel model)
        {
            // TODO: save stuff
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
    }
}
