using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarpoolPlanner.ViewModel
{
    public class SetPasswordViewModel : ViewModelBase
    {
        public long UserId { get; set; }

        public string NewPassword { get; set; }

        public string OldPassword { get; set; }
    }
}