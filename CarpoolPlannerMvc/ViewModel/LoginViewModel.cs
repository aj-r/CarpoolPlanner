using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarpoolPlanner.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}