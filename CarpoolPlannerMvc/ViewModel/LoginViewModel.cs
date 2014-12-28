using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CarpoolPlanner.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        [JsonProperty(PropertyName = "loginNameInput")]
        public string LoginNameInput { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}