using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarpoolPlanner.ViewModel
{
    public class LogFiltersViewModel
    {
        public LogFiltersViewModel()
        {
            Page = 1;
        }

        public int Page { get; set; }
        public string Level { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public long? UserId { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
    }
}