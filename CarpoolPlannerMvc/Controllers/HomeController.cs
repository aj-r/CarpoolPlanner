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
            if (AppUtils.IsUserStatus(UserStatus.Active))
            {
                using (var context = ApplicationDbContext.Create())
                {
                    model.Trips = context.Trips
                        .Where(t => t.UserTrips.Any(ut => ut.UserId == AppUtils.CurrentUser.Id && ut.Attending))
                        .Include(t => t.Recurrences)
                        .ToList();
                    foreach (var tripRecurrence in model.Trips.SelectMany(t => t.Recurrences))
                    {
                        // With EF magic, this method automatically adds the next TripInstance for each recurrence to the Trip
                        var instance = context.GetNextTripInstance(tripRecurrence, ApplicationDbContext.TripInstanceRemovalDelay);
                        context.GetTripInstanceById(instance.Id);
                    }
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
