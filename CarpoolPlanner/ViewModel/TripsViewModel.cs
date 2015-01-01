using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarpoolPlanner.Model;

namespace CarpoolPlanner.ViewModel
{
    public class TripsViewModel : ViewModelBase
    {
        public TripsViewModel()
        {
            Trips = new List<Trip>();
            Create = new CreateTripViewModel();
        }

        public List<Trip> Trips { get; set; }
        public CreateTripViewModel Create { get; set; }
    }
}