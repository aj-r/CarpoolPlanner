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
            Trips = new List<Trip>();
            Users = new List<User>();
        }

        public List<Trip> Trips { get; set; }

        public List<User> Users { get; set; }
    }
}