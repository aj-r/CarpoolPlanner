using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarpoolPlanner.ViewModel
{
    public class CreateUserViewModel : UserViewModel
    {
        /// <summary>
        /// Gets or sets the plain-text password that the user set.
        /// </summary>
        public string Password { get; set; }
    }
}