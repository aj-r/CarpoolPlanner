using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarpoolPlanner.Model;

namespace CarpoolPlanner.ViewModel
{
    public class TripsCombinedViewModel : ViewModelBase
    {
        public TripsCombinedViewModel()
        {
            TripsModel = new TripsViewModel();
            CreateModel = new CreateTripViewModel();
            CreateModel.Trip = new Trip();
            CreateModel.Trip.Recurrences.Add(new TripRecurrence());
        }

        public TripsViewModel TripsModel { get; set; }

        public CreateTripViewModel CreateModel { get; set; }
    }
}