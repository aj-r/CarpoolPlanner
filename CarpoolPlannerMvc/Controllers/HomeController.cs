using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarpoolPlanner.ViewModel;

namespace CarpoolPlanner.Controllers
{
    [Authorize]
    public class HomeController : CarpoolControllerBase
    {
        public ActionResult Index()
        {
            return View(new HomeViewModel());
        }

        [HttpPost]
        public ActionResult Index(HomeViewModel model)
        {
            // TODO: save stuff
            return Ng(model);
        }
    }
}
