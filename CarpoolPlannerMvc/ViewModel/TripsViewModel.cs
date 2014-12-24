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
        }

        public List<Trip> Trips { get; set; }
        public Trip NewTrip { get; set; }

        public string CreateMessage { get; set; }
        public MessageType CreateMessageType { get; set; }
    }
}