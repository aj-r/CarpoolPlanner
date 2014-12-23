using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarpoolPlanner.ViewModel;

namespace CarpoolPlanner.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            if (AppUtils.CurrentUser != null)
                return View(new IndexViewModel());
            else
                return RedirectToLogin();
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {
            // TODO: save stuff
            return Json(model);
        }
    }
}
