using System.Web.Mvc;
using CarpoolPlanner.ViewModel;

namespace CarpoolPlanner.Controllers
{
    public class TimeZonesController : CarpoolControllerBase
    {
        public ActionResult Index()
        {
            return JsonNet(TimeZoneViewModel.GetAll());
        }
    }
}
