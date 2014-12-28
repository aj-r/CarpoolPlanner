using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarpoolPlanner.Model;
using Newtonsoft.Json;

namespace CarpoolPlanner.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
            UserTrips = new List<UserTrip>();
        }

        [JsonProperty(PropertyName = "userTrips")]
        public List<UserTrip> UserTrips { get; set; }
    }
}