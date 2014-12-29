using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarpoolPlanner.ViewModel
{
    public class UserListViewModel
    {
        public UserListViewModel()
        {
            Users = new List<UserViewModel>();
        }

        public List<UserViewModel> Users { get; set; }
    }
}