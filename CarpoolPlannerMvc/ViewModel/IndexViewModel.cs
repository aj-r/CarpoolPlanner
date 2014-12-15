using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarpoolPlanner.Model;

namespace CarpoolPlanner.ViewModel
{
    public class IndexViewModel : ViewModelBase
    {
        public List<Trip> Trips { get; set; }
    }
}