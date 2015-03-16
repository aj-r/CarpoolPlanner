using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarpoolPlanner.Model;
using NodaTime;
using NodaTime.TimeZones;

namespace CarpoolPlanner.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        public UserViewModel()
        {
            User = new User();
        }

        public UserViewModel(User user)
        {
            User = user;
        }

        public User User { get; set; }
    }
}