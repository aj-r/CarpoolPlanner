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
            var model = new HomeViewModel();
            using (var context = ApplicationDbContext.Create())
            {
                var userTrips = context.GetUserTrips(AppUtils.CurrentUser.Id)
                    .Include(ut => ut.Instances)
                    .Include(ut => ut.Trip.Recurrences)
                    .Where(ut => ut.Attending)
                    .ToList();
                foreach (var tripRecurrence in userTrips.SelectMany(ut => ut.Trip.Recurrences))
                {
                    // With EF magic, this method automatically adds the next TripInstance for each recurrence to the Trip
                    context.GetNextTripInstance(tripRecurrence, ApplicationDbContext.TripInstanceRemovalDelay);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(HomeViewModel model)
        {
            // TODO: save stuff
            return Ng(model);
        }
    }
}
