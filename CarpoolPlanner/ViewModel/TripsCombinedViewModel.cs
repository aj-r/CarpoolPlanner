using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarpoolPlanner.Model;

namespace CarpoolPlanner.ViewModel
{
    public class TripsCombinedViewModel
    {
        public TripsCombinedViewModel()
        {
            TripsModel = new TripsViewModel();
            CreateModel = new SaveTripViewModel();
            CreateModel.Trip = new Trip();
            CreateModel.Trip.Recurrences.Add(new TripRecurrence());
        }

        public TripsViewModel TripsModel { get; set; }

        public SaveTripViewModel CreateModel { get; set; }
    }
}