using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarpoolPlanner.Model;

namespace CarpoolPlanner.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
            Trips = new List<Trip>();
        }

        public List<Trip> Trips { get; set; }
    }
}