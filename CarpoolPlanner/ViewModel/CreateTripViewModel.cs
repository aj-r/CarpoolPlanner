using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarpoolPlanner.Model;

namespace CarpoolPlanner.ViewModel
{
    public class CreateTripViewModel : ViewModelBase
    {
        public Trip Trip { get; set; }
        public Trip CreatedTrip { get; set; }
    }
}