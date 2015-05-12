using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarpoolPlanner.ViewModel;

namespace CarpoolPlanner.Controllers
{
    public class NotificationsController : CarpoolControllerBase
    {
        public ActionResult Index()
        {
            return View(new ViewModelBase());
        }
    }
}
